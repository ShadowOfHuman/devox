using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.BLL.Services
{
    public interface ICRUDService<T> where T : class
    {
        Task<T> Get(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
        Task<long> Create(T item, CancellationToken cancellationToken = default);
        Task Update(T item, CancellationToken cancellationToken = default);
        Task Delete(long id, CancellationToken cancellationToken = default);
    }
}
