using SuperMarket.Entities;
using SuperMarket.Services.Stuffs.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SuperMarket.Persistence.EF.Stuffs
{
    public class EFStuffRepository : StuffRepository
    {
        private readonly EFDataContext _dataContext;

        public EFStuffRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Stuff stuff)
        {
            _dataContext.Stuffs.Add(stuff);
        }

        public Stuff FindById(int id)
        {
            return _dataContext.Stuffs.Find(id);
        }

        public IList<Stuff> GetAllStuff()
        {
            return _dataContext.Stuffs.ToList();
        }

        public void Delete(Stuff stuff)
        {
            _dataContext.Stuffs.Remove(stuff);
        }

        public bool IsExistStuffTitle(string title)
        {
            return _dataContext.Stuffs.Any(_ => _.Title == title);
        }
    }
}
