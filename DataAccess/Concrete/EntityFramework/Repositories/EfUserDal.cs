namespace DataAccess.Concrete.EntityFramework.Repositories

{
    using Abstract;
    using Context;
    using Core.DataAccess.Concrete.EntityFramework;
    using Entities.Concrete;
    using Microsoft.EntityFrameworkCore;

    public class EfUserDal : EfEntityRepositoryBase<User>, IUserDal
    {
        public EfUserDal(DbContext context, DbSet<User> dbSet) : base(context, dbSet)
        {
        }
    }
}
