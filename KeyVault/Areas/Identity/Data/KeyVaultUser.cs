using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using KeyVault.Data;
using Microsoft.AspNetCore.Identity;

namespace KeyVault.Areas.Identity.Data;

// Add profile data for application users by adding properties to the KeyVaultUser class
public class KeyVaultUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<KeyVaultKey> KeyVaultKeys { get; set; }
    public ICollection<KeyVaultKeyUser> AccessibleKeys { get; set; }
}

public class KeyVaultKey
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string URL { get; set; }
    public KeyVaultUser Owner { get; set; }
    public ICollection<KeyVaultKeyUser> AccesiblesUsers { get; set; }
}

public class KeyVaultKeyUser
{
    public string KeyVaultUserId { get; set; }
    public KeyVaultUser KeyVaultUser { get; set; }
    public string KeyVaultKeyId { get; set; }
    public KeyVaultKey KeyVaultKey { get; set; }
}

