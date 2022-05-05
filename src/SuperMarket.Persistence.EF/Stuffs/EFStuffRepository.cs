﻿using SuperMarket.Entities;
using SuperMarket.Services.Stuffs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Stuffs
{
    public class EFStuffRepository:StuffRepository
    {
        private readonly EFDataContext _dataContext;

        public EFStuffRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Stuff stuff)
        {
            _dataContext.Add(stuff);
        }

        public Stuff FindById(int id)
        {
            return _dataContext.Stuffs.Find(id);
        }

        public IList<Stuff> GetAllStuff()
        {
            return _dataContext.Stuffs.ToList();
        }

        public bool IsExistStuffTitle(string title)
        {
            return _dataContext.Stuffs.Any(_ => _.Title == title);
        }
    }
}
