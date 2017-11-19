using System.Collections.Generic;
using System.Threading.Tasks;
using FUNAPI.Models;

namespace FUNAPI.Repository
{
    //TODO:FIX NAME
    public interface IReadOnlyRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetSingleAsync(int id);
    }
}