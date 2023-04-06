using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages; 
using TextSearchEngine.Models;
using TextSearchEngine.ViewModels;

namespace TextSearchEngine.Pages.Account
{
    public class RegistModel : PageModel
    {
        private readonly DataContext _dbContext;
        private readonly IWebHostEnvironment _webHost;
        public RegistModel(DataContext dbContext, IWebHostEnvironment webHost)
        {
            _dbContext = dbContext;
            _webHost = webHost;
        }
        [BindProperty]
        public UserModel user { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //if (user.UserName == null | user.Email == null | user.PassWord == null)
            //{
            //    return StatusCode(400);
            //}
            if (user.AffrimPassWord != user.PassWord)
            {
                return StatusCode(401);
            }
            await _dbContext.AddAsync(new UserInfo
            {
                Email = user.Email,
                PassWord = user.PassWord,
                UserName = user.UserName
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToPage("./Login");
        }
    }
}
