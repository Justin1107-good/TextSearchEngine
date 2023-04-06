using Masuit.LuceneEFCore.SearchEngine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TextSearchEngine.Models
{
    [Table("MarkDown")]
    public class MarkDown : LuceneIndexableBaseEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        public MarkDown()
        {
            PostDate = DateTime.Now;
        }
        /// <summary>
        /// 文件头
        /// </summary>
        [Display(Name = "文件头")]
        [Required(ErrorMessage = "文件头不能为空！")]
        public string FileTitle { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [Display(Name = "路径")]
        [Required(ErrorMessage = "文件不能为空！")]
        public string File { get; set; } 
        /// <summary>
        /// 文件上传时间
        /// </summary>
        public DateTime PostDate { get; set; }
    }
}
