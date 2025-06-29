using eProfessional.DAL.Context;
using eProfessional.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eProfessional.DAL.Repositories
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        protected readonly EProfessionalContext _context;
        protected readonly DbSet<T> _dbSet;

        public CrudRepository(EProfessionalContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public T GetById(int id) => _dbSet.Find(id);
        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public void Add(T entity) => _dbSet.Add(entity);
        public void Update(T entity) => _context.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public void Save() => _context.SaveChanges();
    }
}
