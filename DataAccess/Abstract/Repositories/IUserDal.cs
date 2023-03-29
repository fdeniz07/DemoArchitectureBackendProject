namespace DataAccess.Abstract
{
    using Core.DataAccess.Abstract;
    using Entities.Concrete;

    public interface IUserDal :IEntityRepository<User>
    {

    }
}
