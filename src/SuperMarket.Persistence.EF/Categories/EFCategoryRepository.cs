﻿using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Categories
{
    public class EFCategoryRepository:CategoryRepository
    {
        private readonly EFDataContext _dataContext;
        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public bool IsExistCategoryTitle(string title)
        {
            return _dataContext.Categories.Any(_ => _.Title == title);
        }
    }
}
