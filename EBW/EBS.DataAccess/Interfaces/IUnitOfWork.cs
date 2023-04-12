using EBW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<CoverType> CoverType { get; }
        IRepository<Category> Category { get; }
        IRepository<Author> Author { get; }
        IRepository<Product> Product { get; }
        IRepository<ShoppingCart> ShoppingCart { get; }
        Task SaveAsync();
    }
}
