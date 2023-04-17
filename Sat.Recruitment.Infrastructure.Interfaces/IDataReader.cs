using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Infrastructure.Interfaces
{
    /// <summary>
    /// Generic data reader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataReader<out T> where T : class
    {
        public Task ReadData();
        public IEnumerable<T> GetData();

    }
}