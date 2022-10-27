using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using KeyVault.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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


    private string encrypt(string s)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes a = Aes.Create())
        {
            a.Key = Encoding.UTF8.GetBytes(TrimStringForKey(Owner.Id));
            a.IV = iv;
            ICryptoTransform encryptor = a.CreateEncryptor(a.Key, a.IV);
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)stream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(s);
                    }

                    array = stream.ToArray();
                }
            }
        }
        return Convert.ToBase64String(array);
    }

    private string decrypt(string s)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(s);

        using (Aes a = Aes.Create())
        {
            a.Key = Encoding.UTF8.GetBytes(TrimStringForKey(Owner.Id));
            a.IV = iv;
            ICryptoTransform decryptor = a.CreateDecryptor(a.Key, a.IV);

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)stream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    public void Encrypt()
    {
        if (Owner is null || string.IsNullOrEmpty(Owner.Id)) return;
        Password = encrypt(Password);
        Username = encrypt(Username);
        
    }

    public void Decrypt()
    {
        if (Owner is null || string.IsNullOrEmpty(Owner.Id)) return;
        Password = decrypt(Password);
        Username = decrypt(Username);        
    }

    private static string TrimStringForKey(string s)
    {
        Regex r = new Regex("(?:[^a-zA-Z0-9 ]|(?<=['\"])s)",RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return r.Replace(s, string.Empty);
    }
}

public class KeyVaultKeyUser
{
    public string KeyVaultUserId { get; set; }
    public KeyVaultUser KeyVaultUser { get; set; }
    public string KeyVaultKeyId { get; set; }
    public KeyVaultKey KeyVaultKey { get; set; }
}

