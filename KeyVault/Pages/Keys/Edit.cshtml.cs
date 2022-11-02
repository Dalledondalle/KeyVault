using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeyVault.Data;
using KeyVault.Areas.Identity.Data;

namespace KeyVault.Pages.Keys
{
    public class EditModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public EditModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

        [BindProperty]
        public KeyVaultKey KeyVaultKey { get; set; } = default!;

        [BindProperty]
        public List<KeyVaultUser> GuestUsers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.KeyVaultKeys == null)
            {
                return NotFound();
            }

            var keyvaultkey =  await _context.KeyVaultKeys.Include(x => x.Owner).FirstOrDefaultAsync(m => m.Id == id);
            if (keyvaultkey == null)
            {
                return NotFound();
            }
            keyvaultkey.Decrypt();
            KeyVaultKey = keyvaultkey;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var keyvaultkey = await _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).FirstOrDefaultAsync(m => m.Id == KeyVaultKey.Id);

            keyvaultkey.Password = KeyVaultKey.Password;
            keyvaultkey.Username = KeyVaultKey.Username;
            keyvaultkey.URL = KeyVaultKey.URL;
            keyvaultkey.Encrypt();

            _context.Attach(keyvaultkey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeyVaultKeyExists(KeyVaultKey.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool KeyVaultKeyExists(string id)
        {
          return _context.KeyVaultKeys.Any(e => e.Id == id);
        }
    }
}
