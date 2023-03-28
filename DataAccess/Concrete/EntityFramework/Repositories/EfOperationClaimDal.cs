namespace DataAccess.Concrete.EntityFramework
{
    using Abstract;
    using Context;
    using Core.DataAccess.EntityFramework;
    using Entities.Concrete;

    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, PostgreDbContext>, IOperationClaimDal
    {
       
    }
}
