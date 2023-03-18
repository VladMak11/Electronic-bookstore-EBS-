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
        Task SaveAsync();
    }
}
