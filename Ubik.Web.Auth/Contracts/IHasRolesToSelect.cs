using Ubik.Web.Auth.ViewModels;

namespace Ubik.Web.Auth.Contracts
{
    public interface IHasRolesToSelect
    {
        RoleViewModel[] AvailableRoles { get; }
    }
}