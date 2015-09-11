using System.Collections.Generic;

namespace Ubik.Web.Auth.ViewModels
{
    public class UserRowViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<RoleRowViewModel> Roles { get; set; }
    }
}