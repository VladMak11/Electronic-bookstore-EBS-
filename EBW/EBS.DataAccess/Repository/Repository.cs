using EBW.DataAccess;
using EBW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace EBW.DataAccess
{
    public class Repository<T> : IRepository<T> where T : IndetifiedModel
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDBContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? paramFilter = null)
        {
            IQueryable<T> curentQuery = _dbSet; 

            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> paramFilter)
        {
            IQueryable<T> curentQuery = _dbSet;
            curentQuery = curentQuery.Where(paramFilter);
            return await curentQuery.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var item = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            EntityEntry entityEntry = _db.Entry<T>(item);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task UpdateAsync(T item)
        {
            EntityEntry entityEntry = _db.Entry<T>(item);
            entityEntry.State= EntityState.Modified;
        }

        //public void RemoveRange(IEnumerable<T> item)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
