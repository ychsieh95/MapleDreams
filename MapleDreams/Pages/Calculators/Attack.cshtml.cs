using System;
using System.Text.Json;
using MapleDreams.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MapleDreams.Pages.Calculators
{
public class AttackModel : PageModel
    {
        // --------------------------------------------------
        // -- BindProperty
        // --------------------------------------------------
        [BindProperty] public string Data { get; set; }
        [BindProperty] public int WeaponSelect { get; set; }
        [BindProperty] public decimal WeaponProficiency { get; set; }
        [BindProperty] public int MapleWarriorLevel { get; set; }
        [BindProperty(SupportsGet = true)] public string PageId { get; set; }
        [BindProperty] public IFormFile UploadedFile { get; set; }

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        public AttackCalculator AttackCalculator{ get; private set; } = new();
        public List<SelectListItem> WeaponType { get; private set; }
        public decimal MinAttack { get; private set; }
        public decimal MaxAttack { get; private set; }
        private IWebHostEnvironment appEnvironment { get; set; }

        // --------------------------------------------------
        // -- 
        // --------------------------------------------------
        [TempData]
        public string[] StatusMessage { get; set; }

        public AttackModel(IWebHostEnvironment appEnvironment) => this.appEnvironment = appEnvironment;

        public void OnGet()
        {
            LoadData();
            ParseData();
            RemoveInvalidData();

            WeaponType = [];
            for (int i = 0; i < AttackCalculator.WeaponTypes.Count; i++)
            {
                WeaponType.Add(new SelectListItem(text: AttackCalculator.WeaponTypes[i], value: i.ToString(), selected: false));
            }
            WeaponType[WeaponSelect].Selected = true;

            AttackCalculator.Calculate();
            MaxAttack = AttackCalculator.MaxAttack;
            MinAttack = AttackCalculator.MinAttack;

            if (StatusMessage != null)
            {
                ModelState.AddModelError(StatusMessage[0], StatusMessage[1]);
                StatusMessage = null;
            }
        }

        public IActionResult OnPost()
        {
            SaveData();
            return RedirectToPage(new { PageId = PageId });
        }

        public IActionResult OnPostDownload()
        {
            LoadData();
            ParseData();
            string filePath = $@"{appEnvironment.WebRootPath}/calculators/attack/{PageId}.dat";
            string mimeType = "text/plain";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType, $"{DateTime.Now.ToString("yyyyMMdd_HHmmss_")}_{PageId}.dat");
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                PageId = Guid.NewGuid().ToString();
                var uploadPath = $@"{appEnvironment.WebRootPath}/calculators/attack";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, $"{PageId}.dat");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                try
                {
                    StatusMessage = ["Success", "檔案上傳成功。"];
                }
                catch (Exception)
                {
                    StatusMessage = ["Error", "檔案上傳成功但內容解析失敗，請檢查上傳檔案是否正確。"];
                }
            }
            else
            {
                StatusMessage = ["Warning", "請選擇上傳檔案。"];
            }
            return RedirectToPage(new { PageId = PageId, StatusMessage = StatusMessage });
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(PageId))
            {
                PageId = Guid.NewGuid().ToString();
            }

            string filePath = $@"{appEnvironment.WebRootPath}/calculators/attack/{PageId}.dat";
            string directory = Path.GetDirectoryName(filePath);
            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                var attackCalculator = JsonSerializer.Deserialize<AttackCalculator>(json);
                Data = attackCalculator.Data;
            }
            else
            {
                Data = string.Empty;
            }
        }

        private void SaveData()
        {
            if (string.IsNullOrEmpty(PageId))
            {
                PageId = Guid.NewGuid().ToString();
            }

            string filePath = $@"{appEnvironment.WebRootPath}/calculators/attack/{PageId}.dat";
            string diaPath = Path.GetDirectoryName(filePath);
            if (Directory.Exists(diaPath) == false)
            {
                Directory.CreateDirectory(diaPath);
            }

            AttackCalculator.Data = Data;
            AttackCalculator.DateTime = DateTime.Now;

            string json = JsonSerializer.Serialize(AttackCalculator);
            System.IO.File.WriteAllText(filePath, json);
        }

        private void ParseData()
        {
            // Ensure Equipment is initialized from Data
            AttackCalculator.FromData(Data);

            // Parse for Data string to extract
            if (string.IsNullOrEmpty(Data))
            {
                WeaponSelect = 0;
                WeaponProficiency = 10;
                MapleWarriorLevel = 0;
            }
            else
            {
                WeaponSelect = AttackCalculator.WeaponSelect;
                WeaponProficiency = AttackCalculator.WeaponProficiency;
                MapleWarriorLevel = AttackCalculator.MapleWarriorLevel;
            }
        }

        private void RemoveInvalidData()
        {
            string dirPath = $@"{appEnvironment.WebRootPath}/calculators/attack";
            if (Directory.Exists(dirPath))
            {
                foreach (var file in Directory.GetFiles(dirPath, "*.dat"))
                {
                    try
                    {
                        var json = System.IO.File.ReadAllText(file);
                        var attackCalculator = JsonSerializer.Deserialize<AttackCalculator>(json);
                        if (DateTime.Now - attackCalculator.DateTime > TimeSpan.FromDays(7))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
