using JiebaNet.Segmenter.Common;
using Masuit.LuceneEFCore.SearchEngine;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TextSearchEngine.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Text;
using OpenXmlPowerTools;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace TextSearchEngine.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ISearchEngine<DataContext> _searchEngine;
        private readonly ILuceneIndexer _luceneIndexer;
        private readonly IWebHostEnvironment _webHost;
        private readonly DataContext _context;
        public HomeController(ISearchEngine<DataContext> searchEngine, ILuceneIndexer luceneIndexer, IWebHostEnvironment webHost, DataContext context)
        {
            _searchEngine = searchEngine;
            _luceneIndexer = luceneIndexer;
            _webHost = webHost;
            _context = context;
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="s">关键词</param>
        /// <param name="page">第几页</param>
        /// <param name="size">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string s, int page, int size)
        {
            //var result = _searchEngine.ScoredSearch<Essay>(new SearchOptions(s, page, size, "Title,Content,Email,Author"));
            var result = _searchEngine.ScoredSearch<Essay>(new SearchOptions(s, page, size, typeof(Essay)));
            return Ok(result);
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        [HttpGet]
        public void CreateIndex()
        {
            //_searchEngine.CreateIndex();//扫描所有数据表，创建符合条件的库的索引
            _searchEngine.CreateIndex(new List<string>() { nameof(Essay) });//创建指定的数据表的索引
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        [HttpPost]
        public void AddIndex(Essay p)
        {
            //更新主键
            p.Id = Guid.NewGuid();
            // 添加到数据库并更新索引
            _searchEngine.Context.Essays.Add(p);
            _searchEngine.SaveChanges();

            //_luceneIndexer.Add(p); //单纯的只添加索引库
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        [HttpDelete]
        public void DeleteIndex(Essay Essay)
        {
            //从数据库删除并更新索引库
            Essay p = _searchEngine.Context.Essays.Find(Essay.Id);
            _searchEngine.Context.Essays.Remove(p);
            _searchEngine.SaveChanges();

            //_luceneIndexer.Delete(p);// 单纯的从索引库移除
        }

        /// <summary>
        /// 更新索引库
        /// </summary>
        /// <param name="Essay"></param>
        [HttpPatch]
        public void UpdateIndex(Essay Essay)
        {
            //从数据库更新并同步索引库
            Essay p = _searchEngine.Context.Essays.Find(Essay.Id);
            // update...
            _searchEngine.Context.Essays.Update(p);
            _searchEngine.SaveChanges();

            //_luceneIndexer.Update(p);// 单纯的更新索引库
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> FileUploadAsync(IFormFile file)
        {
            var filename = string.Empty;
            var filePath = string.Empty;
            if (file != null)
            {
                filePath = Path.Combine(_webHost.WebRootPath, "Files/");
                filename = Path.Combine("Files/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                using (var stream = new FileStream(Path.Combine(_webHost.WebRootPath, filename), FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return Ok(filename);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Account/SignedOut");
        }
        [HttpGet]
        [Route("search")]
        public IActionResult Index()
        {
            String[] arrs = new String[] { "1只青蛙", "2只青蛙", "3只青蛙", "4只青蛙" };
            //由于数组是非动态的，不能进行动态的添加，所有先将它转成list，操作
            ArrayList arrayList = new ArrayList(arrs.ToList());
            arrayList.Add("5只青蛙");
            //我们在将list转换成String[]数组 
            arrs = (string[])arrayList.ToArray(typeof(string));
            StringBuilder stringBuilder = new StringBuilder();
            //输出
            for (int i = 0; i < arrs.Length; i++)
            {
                stringBuilder.Append(arrs[i] + "\t");
            }
            ViewBag.result = stringBuilder.ToString();
            return View();
        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> MarkDownList()
        {
            var result = _context.MarkDowns.ToList();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return Ok(json);

        }
    }
}
