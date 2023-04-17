using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Infrastructure.Interfaces;


namespace Sat.Recruitment.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IList<T> Items = new List<T>();
        private readonly IDataReader<T> _dataReader;

        protected GenericRepository(IDataReader<T> dataReader)
        {
            _dataReader = dataReader;
        }

        public virtual async Task<IEnumerable<T>> AddRange(IEnumerable<T> data)
        {
            await Task.Run(() => data.ToList().ForEach(item => Items.Add(item)));
            return data;
        }

        public virtual async Task<T> Add(T data)
        {
            await Task.Run(() => Items.Add(data));
            return data;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            if (Items.Any()) return await Task.Run(() => Items);
            var data = await Task.Run(()=>_dataReader.GetData());
            AddRange(data);
            return  Items;
            ;
        }

        public virtual Task<T> GetById(object id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> Delete(object id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<T> Update(T data)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Save()
        {
            throw new System.NotImplementedException();
        }
    }
}