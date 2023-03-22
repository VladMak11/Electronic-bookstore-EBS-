using EBW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EBW.DataAccess
{
    public interface IRepository<T> where T : IndetifiedModel
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> paramFilter, params string[] includeProp);
        Task<IEnumerable<T>> GetAllAsync(params string[] includeProp );
        Task AddAsync(T item);
        Task RemoveAsync(int id);
        Task UpdateAsync(T item);

        //void RemoveRange(IEnumerable<T> item);
    }
}
