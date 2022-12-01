namespace Business.Concrete
{
    using Abstract;
    using DataAccess.Abstract;
    using Entities.Concrete;

    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;


        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public void Add(OperationClaim operationClaim)
        {
            //Kontroller
            //DAL => Kayit islemini yap
            _operationClaimDal.Add(operationClaim);

        }
    }
}
