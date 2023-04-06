using Masuit.LuceneEFCore.SearchEngine;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using System.Text;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace TextSearchEngine.Pages.Test
{

    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly DataContext _context;
        public IndexModel(IWebHostEnvironment webHost, DataContext context)
        {
            _context = context;
            _webHost = webHost;
        } 
        public IList<MarkDown> mds { get; set; }
        public void OnGet()
        {
            mds = (IList<MarkDown>)_context.MarkDowns.ToList();
            //  string webRootPath = _webHost.WebRootPath;
            //  string baseDir = Path.Combine(webRootPath, "Files/posts");
            //  string filePath = Path.Combine(_webHost.WebRootPath, "Files/posts/");
            //// string filePath = Path.Combine("Files/posts/");
            //  DirectoryInfo theFolder = new DirectoryInfo(filePath);
            //  files = theFolder.GetFiles("*.md");
        }
    }
}
