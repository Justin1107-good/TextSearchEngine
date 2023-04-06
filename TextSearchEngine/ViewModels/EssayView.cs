using Lucene.Net.Index;
using Masuit.LuceneEFCore.SearchEngine;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TextSearchEngine.ViewModels
{
    public class EssayView
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// 
        [Display(Name = "文章标题")]
        [Required(ErrorMessage = "文章标题不能为空！"), LuceneIndex]
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [Required, MaxLength(24, ErrorMessage = "作者名最长支持24个字符！"), LuceneIndex]
        public string Author { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "文章内容不能为空！"), LuceneIndex(IsHtml = true)]
        public string Content { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// 作者邮箱
        /// </summary>
        [Display(Name = "作者邮箱")]
        [Required(ErrorMessage = "作者邮箱不能为空！"), LuceneIndex]
        public string Email { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [Display(Name = "标签")]
        [StringLength(256, ErrorMessage = "标签最大允许255个字符"), LuceneIndex]
        public string Label { get; set; }

        /// <summary>
        /// 文章关键词
        /// </summary>
        [Display(Name = "文章关键词")]
        [StringLength(256, ErrorMessage = "文章关键词最大允许255个字符"), LuceneIndex]
        public string Keyword { get; set; }
        /// <summary>
        /// 文章图片
        /// </summary>
        [Display(Name = "文章图片")]
        [StringLength(10000, ErrorMessage = "文章图片"), LuceneIndex]
        public string Picture { get; set; }
        public StoredFieldVisitor.Status Status { get; internal set; }
    }
}
