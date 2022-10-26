﻿using System;
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
            if (_context.KeyVaultKeys != null)
            {
                KeyVaultKey = await _context.KeyVaultKeys.ToListAsync();
            }
        }
    }
}
