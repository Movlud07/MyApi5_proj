using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Core.DataAccess.Interfaces
{
    public interface ICrudRepository<T> where T : class, IEntity, new()
    {
        void Add(T insert);
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
        void Delete(int id);
        void Update(int id, T update);
    }
}
