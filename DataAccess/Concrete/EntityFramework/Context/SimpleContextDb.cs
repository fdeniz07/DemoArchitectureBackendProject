namespace DataAccess.Concrete.EntityFramework.Context
{
    using Entities.Concrete;
    using Microsoft.EntityFrameworkCore;

    public class SimpleContextDb:DbContext
    {
        #region Overrides of DbContext
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "User ID=postgres;Password=123456;Host=localhost;Port=5433;Database=DemoDb;");
        }
        #endregion

        public DbSet<User>? Users { get; set; }
        public DbSet<OperationClaim>? OperationClaims { get; set; }
        public DbSet<UserOperationClaim>? UserOperationClaims { get; set; }
    }
}
