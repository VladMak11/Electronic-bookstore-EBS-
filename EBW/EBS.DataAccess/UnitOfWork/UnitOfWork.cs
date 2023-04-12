using EBW.DataAccess;
using EBW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBW.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _db;

        public IRepository<CoverType> CoverType { get; private set; }
        public IRepository<Category> Category { get; private set; }
        public IRepository<Author> Author { get; private set; }
        public IRepository<Product> Product { get; private set; }
        public IRepository<ShoppingCart> ShoppingCart { get; private set; }

        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            CoverType = new Repository<CoverType>(_db);
            Category = new Repository<Category>(_db);
            Author = new Repository<Author>(_db);
            Product = new Repository<Product>(_db);
            ShoppingCart = new Repository<ShoppingCart>(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
