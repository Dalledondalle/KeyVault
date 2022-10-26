using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KeyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KeyVault.Areas.Identity.Data;
using System.Linq;

namespace KeyVault.Pages
{
    public class IndexModel : PageModel
    {
        UserManager<KeyVaultUser> UserManager { get; set; }
        public string ID { get; set; }
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, UserManager<KeyVaultUser> userManager)
        {
            _logger = logger;
            UserManager = userManager;
        }

        public void OnGet()
        { 
            if(User.Identity.IsAuthenticated)
                ID = UserManager.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().Id;
        }
    }
}