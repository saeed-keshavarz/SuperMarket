﻿using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories
{
    public class CategoryAppService:CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CategoryAppService(
            CategoryRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
