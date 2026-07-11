using System.Linq;
using System.Threading;
using MapleDreams.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Mystina.Repositorys;

namespace MapleDreams.Pages.Drops
{
    public class CatalystStoneModel : PageModel
    {
        // --------------------------------------------------
        // -- BindProperty
        // --------------------------------------------------
        [BindProperty(SupportsGet = true)] public string? Name { get; set; }
        [BindProperty(SupportsGet = true)] public bool EnableCatalyst { get; set; }
        [BindProperty(SupportsGet = true)] public List<string> Catalysts { get; set; }
        [BindProperty(SupportsGet = true)] public bool EnableStone { get; set; }
        [BindProperty(SupportsGet = true)] public List<string> Stones { get; set; }
        [BindProperty(SupportsGet = true)] public int Click { get; set; }

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        public List<string> CatalystItems { get; private set; } = new List<string>();
        public List<string> StoneItems { get; private set; } = new List<string>();
        public List<Monster> Monsters { get; private set; } = new List<Monster>();

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        private readonly AppSettings appSettings;

        public CatalystStoneModel(IOptions<AppSettings> appSettings) =>
            this.appSettings = appSettings.Value;

        public async Task OnGetAsync()
        {
            if (Click != 1)
            {
                await IniItems();
            }
            else
            {
                await IniItems();
                if (!string.IsNullOrEmpty(Name))
                {
                    Monsters.RemoveAll(monster => !monster.Name.Contains(Name));
                }
                if (EnableCatalyst)
                {
                    foreach (var catalyst in Catalysts)
                    {
                        Monsters.RemoveAll(monster => !monster.Catalyst.Contains(catalyst));
                    }
                }
                if (EnableStone)
                {
                    foreach (var stone in Stones)
                    {
                        Monsters.RemoveAll(monster => !monster.Stone.Contains(stone));
                    }
                }
            }
        }

        public async Task IniItems()
        {
            var catalystRepository = new CatalystRepository(appSettings.ConnectionStrings.MapleDreamsConnection);
            CatalystItems = new List<string>();
            foreach (var catalyst in await catalystRepository.SelectCatalystsAsync() as List<Catalyst>)
            {
                CatalystItems.Add(catalyst.Name);
            }

            var stoneRepository = new StoneRepository(appSettings.ConnectionStrings.MapleDreamsConnection);
            StoneItems = new List<string>();
            foreach (var stone in await stoneRepository.SelectStonesAsync() as List<Stone>)
            {
                StoneItems.Add(stone.Name);
            }

            var monsterRepository = new MonsterRepository(appSettings.ConnectionStrings.MapleDreamsConnection);
            Monsters = await monsterRepository.SelectMonstersAsync() as List<Monster>;
            foreach (var monster in Monsters)
            {
                monster.Catalyst = monster.Catalyst.Replace(",", "、");
                monster.Stone = monster.Stone.Replace(",", "、");
            }
        }
    }
}
