using System.Data.Entity;

namespace Ubik.EF.Contracts
{
    public interface IRepositoryWithContext<out TDbContext> where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
    }
}