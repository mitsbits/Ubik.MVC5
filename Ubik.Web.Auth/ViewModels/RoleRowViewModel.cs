using System.Collections.Generic;

namespace Ubik.Web.Auth.ViewModels
{
    public class RoleRowViewModel : Selectable
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoleClaimRowViewModel> Claims { get; set; }
    }

    public class RoleClaimRowViewModel
    {
        public string ClaimId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public interface ISelectable
    {
        bool Selected { get; set; }
    }

    public abstract class Selectable : ISelectable
    {
        public bool Selected { get; set; }
    }
}