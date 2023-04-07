using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;
using WatchDog;

namespace TextSearchEngine.Pages.Account
{
    public class UpdateModel : PageModel
    {
        private readonly DataContext _dataContext;
        private readonly ISearchEngine<DataContext> _searchEngine;
        private readonly ILuceneIndexer _luceneIndexer;
        private readonly IWebHostEnvironment _webHost;
        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }
        public Essay ess { get; set; }

        [BindProperty]
        public EssayDto essay { get; set; }
        public IFormFile formFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngine"></param>
        /// <param name="luceneIndexer"></param>
        /// <param name="webHost"></param>
        public UpdateModel(DataContext dataContext, ISearchEngine<DataContext> searchEngine, ILuceneIndexer luceneIndexer, IWebHostEnvironment webHost)
        {
            _dataContext = dataContext;
            _searchEngine = searchEngine;
            _luceneIndexer = luceneIndexer;
            _webHost = webHost;
        }
        public async Task OnGetAsync()
        {
            var essy = await _dataContext.Essays.Where(e => e.Id == Id).FirstOrDefaultAsync();
            if (essy != null)
            {
                ess = essy;
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                WatchLogger.LogError("参数有为空：" + JsonConvert.SerializeObject(ess), eventId: "101");
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
            //从数据库更新并同步索引库
            var p = await _searchEngine.Context.Essays.FindAsync(Id);
            p.Title = essay.Title;
            p.Author = essay.Author;
            p.Content = essay.Content;
            p.Email = User.Identity.Name;
            p.Label = essay.Label;
            p.Keyword = essay.Keyword;
            p.Picture = filename ==null ? p.Picture : filename;
            // update...
            _searchEngine.Context.Essays.Update(p);
            _searchEngine.SaveChanges();
            return RedirectToPage("./MarkDs/index");
        }
    }
}
