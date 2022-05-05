using SuperMarket.Entities;
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
    }
}
