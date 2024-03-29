using DominandoEFCore_Modulo14_RepositoryUow.Data.Repositories;

namespace DominandoEFCore_Modulo14_RepositoryUow.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        private IDepartamentoRepository _departamentoRepository;

        public IDepartamentoRepository DepartamentoRepository => throw new NotImplementedException();

        //public IDepartamentoRepository DepartamentoRepository
        //{
        //    get => _departamentoRepository ?? (_departamentoRepository = new DepartamentoRepository(_context));

        //}

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
