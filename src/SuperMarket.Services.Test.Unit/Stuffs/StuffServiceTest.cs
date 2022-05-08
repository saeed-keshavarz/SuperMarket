using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Stuffs;
using SuperMarket.Services.Stuffs;
using SuperMarket.Services.Stuffs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperMarket.Services.Test.Unit.Stuffs
{
    public class StuffServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly StuffService _sut;
        private readonly StuffRepository _repository;

        public  StuffServiceTest()
        {
            _dataContext =
               new EFInMemoryDatabase()
               .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFStuffRepository(_dataContext);
            _sut = new StuffAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_add_stuff_properly()
        {
            var category = CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

          AddStuffDto dto =  GenerateAddStuffDto(category);

            _sut.Add(dto);

            _dataContext.Stuffs.Should()
                .Contain(_ => 
                _.Title == dto.Title &&
                _.Inventory==dto.Inventory &&
                _.Unit == dto.Unit &&
                _.MinimumInventory==dto.MinimumInventory &&
                _.MaximumInventory==dto.MaximumInventory );
        }

        [Fact]
        public void Add_throw_DuplicateStuffTitleInStuffException_when_add_new_stuff()
        {

        }










        private static AddStuffDto GenerateAddStuffDto(Category category)
        {
            return new AddStuffDto
            {
                Title = "شیر",
                Inventory = 20,
                MinimumInventory = 20,
                MaximumInventory = 50,
                Unit = "پاکت",
                CategoryId = category.Id,
            };
        }

        public static Category CreateCategory(string title)
        {
            return new Category
            {
                Title = title
            };
        }

    }
}
