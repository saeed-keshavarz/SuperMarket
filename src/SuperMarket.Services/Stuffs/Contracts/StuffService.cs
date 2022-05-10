using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System.Collections.Generic;

namespace SuperMarket.Services.Stuffs.Contracts
{
    public interface StuffService : Service
    {
        void Add(AddStuffDto dto);
        Stuff GetById(int id);
        IList<Stuff> GetAllStuff();
        void Update(int id, UpdateStuffDto dto);
        void Delete(int id);
    }
}
