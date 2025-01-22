using Constracts.Domain;
using MongoDB.Driver;

namespace Constracts.Common.Interface
{
    public interface IMongoDBRepository<T> where T : MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);

    }
}
