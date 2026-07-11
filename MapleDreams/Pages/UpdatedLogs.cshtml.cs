using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MapleDreams.Pages
{
    public class UpdatedLogsModel : PageModel
    {
        public Dictionary<DateTime, List<string>> UpdatedLogList { get; private set; } =
            new Dictionary<DateTime, List<string>>()
            {
                {
                    new DateTime(2025, 06, 17),
                    new List<string>()
                    {
                        "新增表攻計算機。"
                    }
                },
                {
                    new DateTime(2024, 05, 20),
                    new List<string>()
                    {
                        "網站上線！提供催化劑、礦石查詢功能。"
                    }
                }
            };

        public void OnGet() { }
    }
}
