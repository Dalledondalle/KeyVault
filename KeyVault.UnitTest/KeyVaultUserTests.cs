using Xunit;
using KeyVault.Areas.Identity.Data;

namespace KeyVault.UnitTest
{
    public class KeyVaultUserTests
    {
        [Theory]
        [InlineData("Pass#123", "ad5bb07a-cb4e-46d0-97f4-b8667329b78f", "opl1lc2TqGUkKMkHBxP31g==")]
        [InlineData("Pass#123", "67hfg4f4-gd85-39gg-5sd4-dfg45df848d6", "Xio2TStMEw+O/UhglfUsag==")]
        [InlineData("123#Pass", "a957e0a9-b1a9-4fdb-a9b7-9605253b41be", "poeAprIp1EGF49gezEtaFA==")]

        public void EncryptPassTests(string password, string ownerId, string encryptedPassword)
        {
            var key = new KeyVaultKey() { Owner = new() { Id = ownerId }, Password = password };

            key.Encrypt();

            Assert.Equal(encryptedPassword, key.Password);
        }


        [Theory]
        [InlineData("Pass#123", "ad5bb07a-cb4e-46d0-97f4-b8667329b78f", "opl1lc2TqGUkKMkHBxP31g==")]
        [InlineData("Pass#123", "67hfg4f4-gd85-39gg-5sd4-dfg45df848d6", "Xio2TStMEw+O/UhglfUsag==")]
        [InlineData("123#Pass", "a957e0a9-b1a9-4fdb-a9b7-9605253b41be", "poeAprIp1EGF49gezEtaFA==")]

        public void DecryptPassTests(string decryptedPassword, string ownerId, string password)
        {
            var key = new KeyVaultKey() { Owner = new() { Id = ownerId }, Password = password, Username = password }; //Username=password is just to fill the username since it cannot be empty

            key.Decrypt();

            Assert.Equal(decryptedPassword, key.Password);
        }

        [Theory]
        [InlineData("Daniel@hotmail.com", "ad5bb07a-cb4e-46d0-97f4-b8667329b78f", "pZuGQt3C1908frWH1Nl4YzL/HpirgzWulTlgYyHGkUk=")]
        [InlineData("DonDalle", "67hfg4f4-gd85-39gg-5sd4-dfg45df848d6", "2QmbCfppudxi/ZiTA0j/sw==")]
        [InlineData("$$SuperUser$$", "a957e0a9-b1a9-4fdb-a9b7-9605253b41be", "fcKlethINXAuu2BmBVYoCA==")]

        public void EncryptUsernameTests(string username, string ownerId, string encryptedUsername)
        {
            var key = new KeyVaultKey() { Owner = new() { Id = ownerId }, Username = username};

            key.Encrypt();

            Assert.Equal(encryptedUsername, key.Username);
        }

        [Theory]
        [InlineData("Daniel@hotmail.com", "ad5bb07a-cb4e-46d0-97f4-b8667329b78f", "pZuGQt3C1908frWH1Nl4YzL/HpirgzWulTlgYyHGkUk=")]
        [InlineData("DonDalle", "67hfg4f4-gd85-39gg-5sd4-dfg45df848d6", "2QmbCfppudxi/ZiTA0j/sw==")]
        [InlineData("$$SuperUser$$", "a957e0a9-b1a9-4fdb-a9b7-9605253b41be", "fcKlethINXAuu2BmBVYoCA==")]

        public void DecryptUsernameTests(string decryptedUsername, string ownerId, string username)
        {
            var key = new KeyVaultKey() { Owner = new() { Id = ownerId }, Username = username, Password = username }; //Password = usernane is just to fill the Password since it cannot be empty

            key.Decrypt();

            Assert.Equal(decryptedUsername, key.Username);
        }
    }
}