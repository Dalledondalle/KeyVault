using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KeyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KeyVault.Areas.Identity.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KeyVault.Pages
{
    public class IndexModel : PageModel
    {
        UserManager<KeyVaultUser> UserManager { get; set; }
        public KeyVaultUser KeyVaultUser { get; set; }
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, UserManager<KeyVaultUser> userManager)
        {
            _logger = logger;
            UserManager = userManager;
        }

        public async Task OnGet()
        { 
            if(UserManager is not null && User.Identity.IsAuthenticated)
                KeyVaultUser = await UserManager.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefaultAsync();
        }
    }
}