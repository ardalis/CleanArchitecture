using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Infrastructure.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _dbContext;

        public DatabaseInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            // _dbContext.Database.Migrate();
            _dbContext.Database.EnsureCreated();
            SeedData.Initialize(_dbContext);
        }
    }
}
