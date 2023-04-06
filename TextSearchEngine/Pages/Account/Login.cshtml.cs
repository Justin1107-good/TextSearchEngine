using Masuit.LuceneEFCore.SearchEngine.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages; 
using System.Security.Claims;
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels; 
using TextSearchEngine.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace TextSearchEngine.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly DataContext _dbContext;
        private readonly IWebHostEnvironment _webHost;
        public LoginModel(DataContext dbContext, IWebHostEnvironment webHost)
        {
            _dbContext = dbContext;
            _webHost = webHost;
        }
        /// <summary>
        /// 
        /// </summary>
        [BindProperty]
        public UserView UserDto { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; private set; }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="returnUrl"></param>
       /// <returns></returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            //if ((returnUrl.Contains("http") | returnUrl.Contains("https"))& returnUrl!=null)
            //{
              
            //}
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            } 
            // 清除现有的外部cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl=null)
        {
            ReturnUrl = returnUrl;
            if (User.Identity.IsAuthenticated) {
                return RedirectToPage("/Account/Index");
            }
            if (ModelState.IsValid)
            { 
                UserInfo? user = _dbContext.UserInfos.Where(x => x.Email == UserDto.Email && x.PassWord == UserDto.PassWord).FirstOrDefault();
                if (user != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("UserName", user.UserName)
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);


                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                     LocalRedirectResult lo= LocalRedirect(Url.GetLocalUrl("/Account/index"));
                    return lo;
                    //return RedirectToPage(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "登陆尝试无效");
                    return Page();

                }
            }
            return Page();
        }

    }
}
