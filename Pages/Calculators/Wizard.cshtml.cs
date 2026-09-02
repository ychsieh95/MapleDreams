using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MapleDreams.Pages.Calculators
{
    public class WizardModel : PageModel
    {
        // --------------------------------------------------
        // -- BindProperty
        // --------------------------------------------------
        [BindProperty(SupportsGet = true)] public double INT { get; set; }
        [BindProperty(SupportsGet = true)] public double AP { get; set; }
        [BindProperty(SupportsGet = true)] public double MonsterHP { get; set; }
        [BindProperty(SupportsGet = true)] public bool Bishop { get; set; }
        [BindProperty(SupportsGet = true)] public bool ArchMage { get; set; }
        [BindProperty(SupportsGet = true)] public bool SkillLevel20 { get; set; }
        [BindProperty(SupportsGet = true)] public bool SkillLevel30 { get; set; }

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        public double Blizzard20 { get; private set; }
        public double Blizzard30 { get; private set; }
        public double Genesis20 { get; private set; }
        public double Genesis30 { get; private set; }

        public Dictionary<string, double> OneHitAPDict { get; private set; } = new Dictionary<string, double>()
        {
            {"Genesis20", 0},
            {"Genesis20Type", 0},
            {"Genesis30", 0},
            {"Genesis30Type", 0},
            {"Blizzard20", 0},
            {"Blizzard20Extend1.10", 0},
            {"Blizzard20Extend1.25", 0},
            {"Blizzard20Extend1.30", 0},
            {"Blizzard20Type", 0},
            {"Blizzard20TypeExtend1.10", 0},
            {"Blizzard20TypeExtend1.25", 0},
            {"Blizzard20TypeExtend1.30", 0},
            {"Blizzard30", 0},
            {"Blizzard30Extend1.10", 0},
            {"Blizzard30Extend1.25", 0},
            {"Blizzard30Extend1.30", 0},
            {"Blizzard30Type", 0},
            {"Blizzard30TypeExtend1.10", 0},
            {"Blizzard30TypeExtend1.25", 0},
            {"Blizzard30TypeExtend1.30", 0},
        };

        public int Blizzard20OneHitAP { get; private set; }
        public int Blizzard30OneHitAP { get; private set; }
        public int Genesis20OneHitAP { get; private set; }
        public int Genesis30OneHitAP { get; private set; }

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        public const double BlizzardATK20 = 520;
        public const double BlizzardATK30 = 570;
        public const double GenesisATK20 = 620;
        public const double GenesisATK30 = 700;

        public void OnGet()
        {
            Bishop = true;
            ArchMage = true;
            SkillLevel20 = true;
            SkillLevel30 = true;
        }

        public void OnPost()
        {
            double baseAP = (((AP * AP / 1000) + (AP * 0.54)) / 30) + (INT / 200);
            Blizzard20 = baseAP * BlizzardATK20 * 1.4;
            Blizzard30 = baseAP * BlizzardATK30 * 1.4;
            Genesis20 = baseAP * GenesisATK20;
            Genesis30 = baseAP * GenesisATK30;

            OneHitAPDict["Genesis20"] = CalcOneHitAP(GenesisATK20);
            OneHitAPDict["Genesis20Type"] = CalcOneHitAP(GenesisATK20 * 1.5);
            OneHitAPDict["Genesis30"] = CalcOneHitAP(GenesisATK30);
            OneHitAPDict["Genesis30Type"] = CalcOneHitAP(GenesisATK30 * 1.5);

            OneHitAPDict["Blizzard20"] = CalcOneHitAP(BlizzardATK20 * 1.4);
            OneHitAPDict["Blizzard20Extend1.10"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.10);
            OneHitAPDict["Blizzard20Extend1.25"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.25);
            OneHitAPDict["Blizzard20Extend1.30"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.30);
            OneHitAPDict["Blizzard20Type"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.5);
            OneHitAPDict["Blizzard20TypeExtend1.10"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.5 * 1.10);
            OneHitAPDict["Blizzard20TypeExtend1.25"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.5 * 1.25);
            OneHitAPDict["Blizzard20TypeExtend1.30"] = CalcOneHitAP(BlizzardATK20 * 1.4 * 1.5 * 1.30);

            OneHitAPDict["Blizzard30"] = CalcOneHitAP(BlizzardATK30 * 1.4);
            OneHitAPDict["Blizzard30Extend1.10"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.10);
            OneHitAPDict["Blizzard30Extend1.25"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.25);
            OneHitAPDict["Blizzard30Extend1.30"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.30);
            OneHitAPDict["Blizzard30Type"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.5);
            OneHitAPDict["Blizzard30TypeExtend1.10"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.5 * 1.10);
            OneHitAPDict["Blizzard30TypeExtend1.25"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.5 * 1.25);
            OneHitAPDict["Blizzard30TypeExtend1.30"] = CalcOneHitAP(BlizzardATK30 * 1.4 * 1.5 * 1.30);
        }

        public double CalcOneHitAP(double skillRratio) =>
            ((-1 * (skillRratio * 18 / 1000)) + Math.Pow((Math.Pow(skillRratio * 18 / 1000, 2) - (4 * (skillRratio / 30000) * (skillRratio * INT / 200 - MonsterHP))), 0.5)) / (2 * (skillRratio / 30000));
    }
}
