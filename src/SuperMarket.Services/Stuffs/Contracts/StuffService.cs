﻿using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Stuffs.Contracts
{
    public interface StuffService : Service
    {
        void Add(AddStuffDto dto);
        void Update(int id, UpdateStuffDto dto);
        IList<Stuff> GetAllStuff();
        void Delete(int id);
        void UpdateInventory(int id, int quantity);
    }
}
