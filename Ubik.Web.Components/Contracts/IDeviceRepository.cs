using Ubik.Web.Components.Domain;

namespace Ubik.Web.Components.Contracts
{
    public interface IDeviceRepository<TKey> 
    {
        Device<TKey> Get(int id);
    }
}
