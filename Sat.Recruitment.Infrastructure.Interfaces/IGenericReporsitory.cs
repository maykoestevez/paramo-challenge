using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Infrastructure.Interfaces
{
    /// <summary>
    /// Generic repository for data operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> Add(T data);
        public Task<IEnumerable<T>> AddRange(IEnumerable<T> data);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(object id);
        public Task<bool> Delete(object id);
        public Task<T> Update(T data);
        public Task Save();
    }
}