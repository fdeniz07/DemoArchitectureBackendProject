namespace DataAccess.Concrete.EntityFramework.Repositories
{
    using Abstract.Repositories;
    using Core.DataAccess.Concrete.EntityFramework;
    using Entities.Concrete;
    using Microsoft.EntityFrameworkCore;

    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim>, IOperationClaimDal
    {
        public EfOperationClaimDal(DbContext context, DbSet<OperationClaim> dbSet) : base(context, dbSet)
        {
        }
    }
}
