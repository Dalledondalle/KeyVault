using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KeyVault.Data;
using KeyVault.Areas.Identity.Data;

namespace KeyVault.Pages.Keys
{
    public class DeleteModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public DeleteModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

        [BindProperty]
      public KeyVaultKey KeyVaultKey { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            var owner = _context.Users.FirstOrDefault(x => x.NormalizedEmail == User.Identity.Name.ToUpper());
            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).FirstOrDefaultAsync(m => m.Id == id);
            if (owner is null || keyvaultkey.Owner.Id != owner.Id)
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

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            var owner = _context.Users.FirstOrDefault(x => x.NormalizedEmail == User.Identity.Name.ToUpper());
            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).FirstOrDefaultAsync(m => m.Id == id);
            if (owner is null || keyvaultkey.Owner.Id != owner.Id)
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
                _context.KeyVaultKeys.Remove(KeyVaultKey);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
