﻿using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Stuffs.Contracts
{
    public interface StuffRepository : RepoSitory
    {
        void Add(Stuff stuff);
        bool IsExistStuffTitle(string title);
        Stuff FindById(int id);
        IList<Stuff> GetAllStuff();
        void Delete(Stuff stuff);
    }
}