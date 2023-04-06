using Masuit.LuceneEFCore.SearchEngine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TextSearchEngine.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo : LuceneIndexableBaseEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        public UserInfo(){
            PostDate = DateTime.Now;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "邮箱不能为空！")]
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空！")]
        public string PassWord { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        [Required(ErrorMessage = "姓名不能为空！")]
        public string UserName { get; set; }
        /// <summary>
        /// 账号创建时间
        /// </summary>
        public DateTime PostDate { get; set; }
    }
}
