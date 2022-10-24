using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KeyVault.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KeyVault.Areas.Identity.Data;
using System.Linq;

namespace KeyVault.Pages
{
    public class IndexModel : PageModel
    {
        UserManager<KeyVaultUser> UserManager { get; set; }
        public string ID { get; set; }
        private List<DummyKey> dummyKeys = new List<DummyKey>()
        {
        new DummyKey() { Username = "Dalle", Password = "asd123", OwnerID = "64f47033-bbcc-41b2-9b9b-c24095d27619" },
        new DummyKey() { Username = "Laambi", Password = "14432" },
        new DummyKey() { Username = "Dalle&Laambi", Password = "sdff3wf" },
        new DummyKey() { Username = "Laambi&Dalle", Password = "12rfasf", OwnerID = "", Guests = new(){ "64f47033-bbcc-41b2-9b9b-c24095d27619" } },
        new DummyKey() { Username = "xcvdxv", Password = "234sdf" },
        new DummyKey() { Username = "qwerty", Password = "dfge4r" }
    };
        public List<DummyKey> Keys { get; set; } = new();
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, UserManager<KeyVaultUser> userManager)
        {
            _logger = logger;
            UserManager = userManager;
        }

        [Authorize]
        public void OnGet()
        { 
            ID = UserManager.Users.Where(x => x.Email == User.Identity.Name).First().Id;     
            Keys = dummyKeys.Where(x => x.OwnerID == ID || x.Guests.Contains(ID) ).ToList();
        }
    }
}