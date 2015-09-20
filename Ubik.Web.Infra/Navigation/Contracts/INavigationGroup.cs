namespace Ubik.Web.Infra.Navigation.Contracts
{
    public interface INavigationGroup
    {
        string Display { get; }
        string Key { get; }
        string Description { get; }
    }
}