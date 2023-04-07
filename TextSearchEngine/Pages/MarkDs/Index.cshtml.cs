using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TextSearchEngine.Models;

namespace TextSearchEngine.Pages.MarkDs
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IWebHostEnvironment _webHost;
        private readonly DataContext _context;
        public IndexModel(IWebHostEnvironment webHost, DataContext context)
        {
            _context = context;
            _webHost = webHost;
        }
        /// <summary>
        /// 
        /// </summary>
        public IList<MarkDown> mds { get; set; }
       /// <summary>
       /// 文件列表
       /// </summary>
        public void OnGet()
        {
            mds = (IList<MarkDown>)_context.MarkDowns.ToList();
        }
    }
}
