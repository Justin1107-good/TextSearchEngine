using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;
using WatchDog;

namespace TextSearchEngine.Pages.Account
{
    /// <summary>
    /// 
    /// </summary>
    public class AddModel : PageModel
    {
        private readonly ISearchEngine<DataContext> _searchEngine;
        private readonly ILuceneIndexer _luceneIndexer;
        private readonly IWebHostEnvironment _webHost;
        [BindProperty]
        public EssayDto essay { get; set; }
        public IFormFile formFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngine"></param>
        /// <param name="luceneIndexer"></param>
        /// <param name="webHost"></param>
        public AddModel(ISearchEngine<DataContext> searchEngine, ILuceneIndexer luceneIndexer, IWebHostEnvironment webHost)
        {
            _searchEngine = searchEngine;
            _luceneIndexer = luceneIndexer;
            _webHost = webHost;
        } 
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                WatchLogger.LogError("参数有为空："+JsonConvert.SerializeObject(essay), eventId: "101");
                return Page();
            }
            
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserName"] = User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
                ViewData["Name"] = User.Identity.Name;
            }
            var filename = string.Empty;
            var filePath = string.Empty;
            if (formFile != null)
            {
                filePath = Path.Combine(_webHost.WebRootPath, "Files/");
                filename = Path.Combine("Files/", Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                using (var stream = new FileStream(Path.Combine(_webHost.WebRootPath, filename), FileMode.CreateNew))
                {
                    formFile.CopyTo(stream);
                }
            }
            //string file = FileUpload(essay.Picture);
            Essay p = new Essay
            {
                Title = essay.Title,
                Author = essay.Author,
                Content = essay.Content,
                Email = User.Identity.Name,
                Label = essay.Label,
                Keyword = essay.Keyword,
               Picture = filename
            };
            //更新主键
            p.Id = Guid.NewGuid();
            // 添加到数据库并更新索引
            _searchEngine.Context.Essays.Add(p);
            _searchEngine.SaveChanges();

            return RedirectToPage("./Index");
        }
        //public string OnPostFileUpload(IFormFile file)
        //{
        //    var filename = string.Empty;
        //    var filePath = string.Empty;
        //    if (file != null)
        //    {
        //        filePath = Path.Combine(_webHost.WebRootPath, "Files/");
        //        filename = Path.Combine("Files/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
        //        using (var stream = new FileStream(Path.Combine(_webHost.WebRootPath, filename), FileMode.CreateNew))
        //        {
        //            file.CopyToAsync(stream);
        //        }
        //    }
        //    return filename;
        //}
    }
}
