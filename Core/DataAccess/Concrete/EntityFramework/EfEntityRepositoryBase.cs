using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.Concrete.EntityFramework
{


    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, new() //referans tip olmali, instance alinabilmeli
                                     // where TContext : DbContext, new()

    {

        protected readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EfEntityRepositoryBase(DbContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }


        public async Task<TEntity> AddAsync(TEntity entity)
        {

            await _dbSet.AddAsync(entity);
            return entity;

        }


        public void Add(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();

            //using (var context = new TContext()) //Using kullanmamizin nedeni: islem bittikten sonra bellekten atilmasi
            //{
            //    var addedEntity = context.Entry(entity);
            //    addedEntity.State = EntityState.Added;
            //    context.SaveChanges();
            //}
        }



        public void Delete(TEntity entity)
        {

                var deletedEntity = _context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                _context.SaveChanges();
            
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {

                return _context.Set<TEntity>().SingleOrDefault(filter);

        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {

                return filter == null
                    ? _context.Set<TEntity>().ToList()
                    : _context.Set<TEntity>().Where(filter).ToList();

        }

        public void Update(TEntity entity)
        {

                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                _context.SaveChanges();
            
        }
    }
}
