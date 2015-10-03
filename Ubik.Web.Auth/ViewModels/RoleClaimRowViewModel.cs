using Ubik.Infra.Contracts;

namespace Ubik.Web.Auth.ViewModels
{
    public class RoleClaimRowViewModel : ISelectable
    {
        public string ClaimId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}