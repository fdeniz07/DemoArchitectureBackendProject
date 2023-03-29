namespace Business.DependenyResolvers.Autofac
{
    using Abstract;
    using Concrete;
    using DataAccess.Abstract.Repositories;
    using DataAccess.Concrete.EntityFramework;
    using global::Autofac;

    public class AutofacBusinessModule:Module
    {
        #region Overrides of Module

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();
        }

        #endregion
    }
}
