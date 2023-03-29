using Entities.Concrete;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        void Add(OperationClaim operationClaim);
        void AddAsync(OperationClaim operationClaim);
    }
}
