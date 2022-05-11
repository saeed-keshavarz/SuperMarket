using SuperMarket.Infrastructure.Application;

namespace SuperMarket.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dataContext;

        public EFUnitOfWork(EFDataContext dataConext)
        {
            _dataContext = dataConext;
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
        }
    }
}
