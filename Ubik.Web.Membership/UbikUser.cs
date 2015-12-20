using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Web.Membership
{
    public class UbikUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

    }
        
    public class UbikRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
