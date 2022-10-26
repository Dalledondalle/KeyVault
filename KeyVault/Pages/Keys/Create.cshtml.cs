using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KeyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KeyVault.Areas.Identity.Data;

namespace KeyVault.Pages.Keys
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;
        UserManager<KeyVaultUser> UserManager { get; set; }

        public CreateModel(KeyVault.Data.KeyVaultContext context, UserManager<KeyVaultUser> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public KeyVaultKey KeyVaultKey { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {          
            KeyVaultKey.Owner = UserManager.Users.Where(x => x.NormalizedEmail == User.Identity.Name.ToUpper()).First();
            KeyVaultKey.Id = Guid.NewGuid().ToString();

            _context.KeyVaultKeys.Add(KeyVaultKey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
