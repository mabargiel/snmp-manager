using System.Threading.Tasks;

namespace BuildingBlocks.Data
{
    public interface IRepository<TEntity> where TEntity: class
    {
        Task SaveChangesAsync();
    }
}
