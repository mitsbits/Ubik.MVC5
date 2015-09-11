namespace Ubik.Web.Auth.Contracts
{
    public interface IViewModelBuilder<in TEntity, TViewModel>
    {
        TViewModel CreateFrom(TEntity entity);

        void Rebuild(TViewModel model);
    }
}