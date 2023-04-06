
using Masuit.LuceneEFCore.SearchEngine;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;
using WatchDog;

namespace TextSearchEngine.Pages.Essays
{
    public class IndexModel : PageModel
    {
        private readonly ISearchEngine<DataContext> _searchEngine;
        private readonly ILuceneIndexer _luceneIndexer;
        private readonly IWebHostEnvironment _webHost;
        public IndexModel(ISearchEngine<DataContext> searchEngine, ILuceneIndexer luceneIndexer, IWebHostEnvironment webHost)
        {
            _searchEngine = searchEngine;
            _luceneIndexer = luceneIndexer;
            _webHost = webHost;
        }
        /// <summary>
        /// 
        /// </summary>
        public IScoredSearchResultCollection<Essay>? essays = null;
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
        /// <param name="s"></param>
        public void OnGet(string s)
        {
            int page = 1; int size = 5;
            if (!string.IsNullOrEmpty(s))
            {
                essays = _searchEngine.ScoredSearch<Essay>(new SearchOptions(s, page, size, typeof(Essay)));
                facses = essays.Results.Count == 0 ? facses = -1 : essays.Results.Count;
               
                WatchLogger.LogWarning(JsonConvert.SerializeObject(essays));
                //WatchLogger.LogError(res.Content, eventId: reference);
            }
            else
            { 
                facses = 0;
                WatchLogger.LogError("查询条件为空", eventId: "101");
            }
             RedirectToPage("./Essays/index");
        }
    }
}
