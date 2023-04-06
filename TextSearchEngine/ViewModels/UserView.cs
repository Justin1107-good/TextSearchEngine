using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TextSearchEngine.ViewModels
{
    public class UserView
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
  
    }
}
