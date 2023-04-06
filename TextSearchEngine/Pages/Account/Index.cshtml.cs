using Masuit.LuceneEFCore.SearchEngine;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages; 
using TextSearchEngine.Models;

namespace TextSearchEngine.Pages.Account
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly DataContext _dbContext;
        private readonly IWebHostEnvironment _webHost;
        public IndexModel(DataContext dbContext, IWebHostEnvironment webHost)
        {
            _dbContext = dbContext;
            _webHost = webHost;
        }
      
        /// <summary>
        /// 
        /// </summary>
        public List<Essay>? essays = null;
        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public int page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int facses;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void OnGet()
        {
            // int page = 1; int size = 5;
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserName"] = User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
                ViewData["Name"] = User.Identity.Name;
            }
            essays = _dbContext.Essays.OrderBy(c => c.PostDate).ToList();
            // facses = essays.Count == 0 ? facses = -1 : essays.Count;

          //  return OkResult(essays);
        }
    }
}
