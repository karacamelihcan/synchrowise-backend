namespace Synchrowise.Database.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SynchrowiseDbContext _context;

        public UnitOfWork(SynchrowiseDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}