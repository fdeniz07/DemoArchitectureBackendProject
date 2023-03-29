namespace DataAccess.Concrete.EntityFramework.Repositories
{
    using Abstract;
    using Context;
    using Core.DataAccess.Concrete.EntityFramework;
    using Entities.Concrete;
    using Microsoft.EntityFrameworkCore;

    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim>, IUserOperationClaimDal
    {
        public EfUserOperationClaimDal(DbContext context, DbSet<UserOperationClaim> dbSet) : base(context, dbSet)
        {
        }
    }
}
