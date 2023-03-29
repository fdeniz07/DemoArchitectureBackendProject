namespace Core.DataAccess.Abstract
{
    using System.Linq.Expressions;

    public interface IEntityRepository<TEntity> where TEntity : class,  new() //where sartlari ile sadece veritabani nesnelerimizin buraya gelecegini belirtiyoruz
    {

        //Buraya yazdigimiz metotlar, tüm entitlerde ortak kullanmak istedigimiz metodlardir.

        //Biz burada cok dinamik bir yapi kuruyoruz. Kullanicinin bilgilerini giriyoruz(filtreden gelen deger),
        //kullanicinin diger bilgilerini cagirmak icinde includProperties kullaniliyoruz, params anahtari ile birden fazla includeproperties getirtip dizeye atiyoruz 


        //Senkron Metotlar

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
        TEntity Get(Expression<Func<TEntity, bool>> filter);

        //Asenkron Metotlar

        Task<TEntity> AddAsync(TEntity entity); //Tekil ekleme
        Task AddRangeAsync(IEnumerable<TEntity> entities);  //Coklu Ekleme
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties);
        //if(isActive==true) predicates.Add();
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IList<TEntity>> GetAllAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties);
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate); // predicate = Lambda
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(TEntity entity); // Biz burada asenkron olarak ayarlayacagiz
        Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> UpdateAsync(TEntity entity);// Biz burada asenkron olarak ayarlayacagiz
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate); //Böyle bir entity daha önceden var mi diye kontrol ediyoruz 
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null); //Tüm entity lerin sayisini dönmek icin de Count kullaniyoruz 
        IQueryable<TEntity> GetAsQueryable(); // Iqueryable olarak verilen enitity'i, bizlere return eder. Bu sayede bizler, herhangi bir sinir olmadan kompleks sorgular olusturabiliyoruz. Farzedelim, GetAsync icerisinde kategoriyi alirken,onun makalelerini include etmek istiyoruz ve makale icerisindeki yorumlari da onun sonrasinda ThenInclude ile include etmek istiyoruz. Burada komplex bir sorgu ile karsilasiyoruz. Bunu normal metotlarimiz icerisinde tamamlayamiyoruz.
    }
}
