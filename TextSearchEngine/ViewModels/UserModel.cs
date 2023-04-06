using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TextSearchEngine.ViewModels
{/// <summary>
/// 
/// </summary>
    public class UserModel
    {
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
        /// 密码
        /// </summary>
        [Display(Name = "验证密码")]
        [Required(ErrorMessage = "验证密码不能为空！")]
        public string AffrimPassWord { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        [Required(ErrorMessage = "姓名不能为空！")]
        public string UserName { get; set; }
    }
}
