using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KeyVault.Areas.Identity.Data;
using KeyVault.Data;

namespace KeyVault.Pages.Keys
{
    public class RemoveModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public RemoveModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

        [BindProperty]
      public KeyVaultKey KeyVaultKey { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var owner = _context.Users.FirstOrDefault(x => x.NormalizedEmail == User.Identity.Name.ToUpper());
            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).FirstOrDefaultAsync(m => m.Id == id);
            if (owner is null || keyvaultkey.AccesiblesUsers.FirstOrDefault(x => x.KeyVaultUserId == owner.Id) is null)
            {
                return NotFound();
            }


            if (keyvaultkey == null)
            {
                return NotFound();
            }
            else
            {
                keyvaultkey.Decrypt();
                KeyVaultKey = keyvaultkey;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var owner = _context.Users.FirstOrDefault(x => x.NormalizedEmail == User.Identity.Name.ToUpper());
            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).FirstOrDefaultAsync(m => m.Id == id);
            var k = keyvaultkey.AccesiblesUsers.Where(x => x.KeyVaultUserId == owner.Id).FirstOrDefault();
            if (owner is null || keyvaultkey.AccesiblesUsers.FirstOrDefault(x => x.KeyVaultUserId == owner.Id) is null || k is null)
            {
                return NotFound();
            }

            if (id == null || _context.KeyVaultKeys == null)
            {
                return NotFound();
            }

            if (keyvaultkey != null)
            {
                KeyVaultKey = keyvaultkey;                
                KeyVaultKey.AccesiblesUsers.Remove(k);

                _context.KeyVaultKeys.Update(KeyVaultKey);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
