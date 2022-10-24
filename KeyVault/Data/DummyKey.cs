namespace KeyVault.Data
{
    public class DummyKey
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OwnerID { get; set; }
        public List<string> Guests { get; set; } = new();
    }
}
