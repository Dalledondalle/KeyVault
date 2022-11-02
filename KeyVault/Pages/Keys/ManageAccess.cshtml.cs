using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KeyVault.Areas.Identity.Data;
using KeyVault.Data;
using Microsoft.EntityFrameworkCore;

namespace KeyVault.Pages.Keys
{
    public class ManageAccessModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public ManageAccessModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.KeyVaultKeys == null)
            {
                return NotFound();
            }

            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).FirstOrDefaultAsync();
            if (keyvaultkey == null)
            {
                return NotFound();
            }
            KeyVaultKey = keyvaultkey;
            return Page();

        }

        [BindProperty]
        public KeyVaultKey KeyVaultKey { get; set; }
        [BindProperty]
        public KeyUser KeyUser { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            KeyVaultKey = _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).Where(x => x.Id == KeyVaultKey.Id).FirstOrDefault();
            var newUser = _context.KeyVaultUsers.Where(x => x.NormalizedEmail == KeyUser.Email.ToUpper()).FirstOrDefault();
            if (newUser is null)
            {
                return Page();
            }
            if (KeyVaultKey.AccesiblesUsers is null)
                KeyVaultKey.AccesiblesUsers = new List<KeyVaultKeyUser>();

            var relation = new KeyVaultKeyUser { KeyVaultKey = KeyVaultKey, KeyVaultUser = newUser };
            
            KeyVaultKey.AccesiblesUsers.Add(relation);
            _context.KeyVaultKeys.Update(KeyVaultKey);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
