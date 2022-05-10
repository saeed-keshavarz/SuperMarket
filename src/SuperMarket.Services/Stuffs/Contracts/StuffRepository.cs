using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Stuffs.Contracts
{
    public interface StuffRepository : Repository
    {
        void Add(Stuff stuff);
        bool IsExistStuffTitle(string title);
        Stuff FindById(int id);
        IList<Stuff> GetAllStuff();
        void Delete(Stuff stuff);
    }
}
