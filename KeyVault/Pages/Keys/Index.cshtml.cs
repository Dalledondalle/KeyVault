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
    public class IndexModel : PageModel
    {
        private readonly KeyVault.Data.KeyVaultContext _context;

        public IndexModel(KeyVault.Data.KeyVaultContext context)
        {
            _context = context;
        }

        public IList<KeyVaultKey> KeyVaultKey { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var name = User.Identity.Name;
            if (_context.KeyVaultKeys != null)
            {
                var id = _context.Users.FirstOrDefault(x => x.NormalizedEmail == User.Identity.Name.ToUpper())?.Id;
                //KeyVaultKey = await _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).ToListAsync();
                KeyVaultKey = await _context.KeyVaultKeys.Include(x => x.Owner).Include(x => x.AccesiblesUsers).Where(x => x.Owner.Id == id || x.AccesiblesUsers.Any(y => y.KeyVaultUserId == id)).ToListAsync();
            }
            foreach (var key in KeyVaultKey)
            {
                key.Decrypt();
            }
        }
    }
}
