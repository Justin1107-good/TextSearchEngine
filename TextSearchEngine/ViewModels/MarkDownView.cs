using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TextSearchEngine.ViewModels
{
    public class MarkDownView
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件头
        /// </summary>
        [Display(Name = "文件头")] 
        public string FileTitle { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [Display(Name = "路径")] 
        public string File { get; set; }
        /// <summary>
        /// 文件上传时间
        /// </summary>
        public DateTime PostDate { get; set; }
    }
}
