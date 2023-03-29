using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.Concrete.EntityFramework
{
    using Abstract;
    using System.Collections.Generic;
    using System.Linq;
    using LinqKit;

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


        // Senkron Metotlar

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

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
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

        public IQueryable<TEntity> GetAsQueryable()
        {
            return _dbSet.AsQueryable(); // UnitOfWork.Blog.GetAsQueryable(); dedigimizde bize blog nesnesini bir Querable nesnesi olarak return ediyor.
        }



        //Asenkron Metotlar

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await (predicate == null ? _dbSet.CountAsync() : _dbSet.CountAsync(predicate)); //predicate null gelirse, o zaman context e set edilmis olan tenitity nin countasync tamamiyle dönüyoruz. Null gelmezse, gelen predicate degeri filtreleme yaparak kullaniciya dönecegiz. Örnek: Kategori tablosunda 6 kayit varsa, toplam 6 degerini predicatesiz olarak dönecegiz. Fakat olur da silinmis kategorileri görmek istersek ve tablomuzda 3 kategori silinmisse; o zaman predicate ile toplam 3 kategori degerini kullaniciya dönüyoruz. Esnek bir yapi kurmus oluyoruz
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<TEntity> GetAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicates != null && predicates.Any()) //buraya yanlislikla bos bir liste gönderme ihtimalimiz var. O yüzden null olma durumunu ve listenin icerisinde verinin varligini kontrol ediyoruz
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate); // isActive==false && isDeleted==true gelebilir
                }
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties.Any()) //bu dizinin icerisinde bir deger varsa, icerisinde döngü ile dönecegiz
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().ToListAsync(); //yukarida dönen degerleri kullanicaya bir liste olarak dönecegiz.
        }

        public async Task<IList<TEntity>> GetAllAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicates != null && predicates.Any()) //buraya yanlislikla bos bir liste gönderme ihtimalimiz var. O yüzden null olma durumunu ve listenin icerisinde verinin varligini kontrol ediyoruz
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate); // isActive==false && isDeleted==true gelebilir
                }
            }

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            //_dbSet.Remove(entity);

            //Remove / Delete islemleri asenkron islemler degildir. Eger asenkron yapilmak istenirse, bizim tarafimizca yeni bir Task olusturulup, icerisine anonim bir metot yazilmalidir.

            await Task.Run(() => { _dbSet.Remove(entity); }); //--> olmalidir.
        }

        public async Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicates.Any())
            {
                var predicateChain = PredicateBuilder.New<TEntity>();
                foreach (var predicate in predicates)
                {
                    //query.Where(predicate) predicate1 && predicate2 && predicate3 && predicateN ve operatörü ile calisir. Bize veya ile ilgili detayli sorgulama islemleri gerektigi icin bir nugetpaket kurmamiz gerekiyor.LinqKit.Microsoft.EntityFrameworkCore isimli paketi kuruyoruz.
                    //query = query.Where(predicate);

                    //predicateChain.Or(predicate) predicate1 || predicate2 || predicate3 || predicateN
                    predicateChain.Or(predicate);
                }

                query = query.Where(predicateChain);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().ToListAsync(); //AsNoTracking metodu bizim icin gereksiz include islemlerini önler veya sadece bizim istedigimiz include ler varsa getirir. Bu sayede gereksiz yere ayni islemler,birbirini tekrar eden islemler (Blog -> Kategori -> Makale -> Yorum -> Blog) gerceklesmeyecegi icin epey bir performans saglamis olacagiz.
        }

        public async Task<TEntity> UpdateAsync(TEntity entity) // Güncelleme islemlerinde ajax icin kücük bir degisiklik <TEntity> olarak geri dönüs tipi belirtiyoruz
        {
            //_context.Entry(entity).State = EntityState.Modified; // Burasi(EntityState.Modified;) cok sütunlü tablolarda kullanmakta cok kullanisli olur.
            // Tek dezavantaji, bir alan bile degisse tüm entity alanlarini güncellemeye calisir

            //entity.name = product.name
            //entity.price = product.price ile yukaridaki performans sorunu azaltilabilir ama cok satira sahip tablolarda ölümcül olabilir.

            //Update islemleri asenkron islemler degildir. Eger asenkron yapilmak istenirse, bizim tarafimizca yeni bir Task olusturulup, icerisine anonim bir metot yazilmalidir.

            await Task.Run(() => { _dbSet.Update(entity); }); //--> olmalidir.

            return entity;
        }

        
       
    }
}
