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
    public class DetailsModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public DetailsModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

      public KeyVaultKey KeyVaultKey { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.KeyVaultKeys == null)
            {
                return NotFound();
            }

            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
