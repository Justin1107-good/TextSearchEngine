using Aspose.Html.Converters;
using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using System.IO;
using System.Text;
using TextSearchEngine.Global;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;
using WatchDog;

namespace TextSearchEngine.Pages.MarkDs
{
    public class AddModel : PageModel
    {
        private readonly ISearchEngine<DataContext> _searchEngine;
        private readonly ILuceneIndexer _luceneIndexer;
        private readonly IWebHostEnvironment _webHost;
        [BindProperty]
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                WatchLogger.LogError("参数有为空：" + JsonConvert.SerializeObject(formFile), eventId: "101");
                return Page();
            }

            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserName"] = User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
                ViewData["Name"] = User.Identity.Name;
            }
            var filename = string.Empty;
            var filePath = string.Empty;
            string fileID = string.Empty;
            string tempPath = string.Empty;
            string htmlPath = string.Empty;
            if (formFile != null)
            {

                #region md to html 
                //string tempPath = System.IO.Path.GetTempPath() + "\\" + "mdtemp";
                //if (!Directory.Exists(tempPath))
                //{
                //    DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
                //    directoryInfo.Create();
                //}
                //fileID = Guid.NewGuid().ToString();
                //string fileTempMD = Path.Combine(tempPath, fileID + Path.GetExtension(formFile.FileName));
                //using (var stream = new FileStream(fileTempMD, FileMode.CreateNew))
                //{
                //    formFile.CopyTo(stream);
                //}
                //string htmlPath = Path.Combine(tempPath, $"{fileID}.html");
                //Converter.ConvertMarkdown(fileTempMD, htmlPath);
                //filePath = Path.Combine(_webHost.WebRootPath, "Files/posts");
                //string[] file = FileHelper.getFilesTypeofHtml(tempPath);

                //// filename = Path.Combine("Files/posts/", Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                //filename = Path.Combine("Files/posts/", fileID + Path.GetExtension(file[0]));
                //using (var stream = new FileStream(Path.Combine(_webHost.WebRootPath, filename), FileMode.OpenOrCreate))
                //{
                //    formFile.CopyTo(stream);
                //}
                //Directory.Delete(tempPath, true);
                #endregion
                //FileHelper.MarkDownToHtml(ref fileID, formFile, ref htmlPath, ref tempPath);
                //FileHelper.SaveFile(formFile, _webHost.WebRootPath, tempPath, fileID, "Files/posts",ref filename); 
                #region md
                filePath = Path.Combine(_webHost.WebRootPath, "Files/posts");
                filename = Path.Combine("Files/posts/", Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

                using (var stream = new FileStream(Path.Combine(_webHost.WebRootPath, filename), FileMode.CreateNew))
                {

                    formFile.CopyTo(stream);
                    stream.Close();


                }


                #endregion
            }
            //string file = FileUpload(essay.Picture);
            MarkDown p = new MarkDown
            {
                FileTitle = Path.GetFileNameWithoutExtension(formFile.FileName),
                File = filename
            };
            //更新主键
            p.Id = Guid.NewGuid();
            // 添加到数据库并更新索引
            _searchEngine.Context.MarkDowns.Add(p);
            _searchEngine.SaveChanges(); 
            if (Path.GetExtension(Path.Combine(_webHost.WebRootPath, filename)).Equals(".docx"))
            {
                using (FileStream fs = new FileStream(Path.Combine(filePath, $"{Path.GetFileNameWithoutExtension(formFile.FileName)}.html"), FileMode.OpenOrCreate))
                {
                    string html = NC.Office.Word.Core.WordHelper.WordToHtml(Path.Combine(_webHost.WebRootPath, filename));
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(html);
                        sw.Close();
                    }

                    fs.Close();
                    fs.Dispose();
                }
            } 
            return RedirectToPage("/Test/Index");
        }
    }
}
