using SuperMarket.Entities;
using SuperMarket.Services.Stuffs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Test.Tools.Stuffs
{
    public static class StuffFactory
    {
        public static Stuff CreateStuff(Category category, string title)
        {
            return new Stuff
            {
                Title = title,
                Inventory = 20,
                MinimumInventory = 20,
                MaximumInventory = 50,
                Unit = "پاکت",
                CategoryId = category.Id,
            };
        }

        public static AddStuffDto GenerateAddStuffDto(Category category, string title)
        {
            return new AddStuffDto
            {
                Title = title,
                Inventory = 20,
                MinimumInventory = 20,
                MaximumInventory = 50,
                Unit = "پاکت",
                CategoryId = category.Id,
            };
        }

        public static UpdateStuffDto GenerateUpdateStuffDto(int categoryId, string title)
        {
            return new UpdateStuffDto
            {
                Title = title,
                Unit = "پاکت",
                MinimumInventory = 10,
                MaximumInventory = 50,
                CategoryId = categoryId,
            };
        }

        public static List<Stuff> CreateStuffsInDataBase(int categoryId)
        {
            return new List<Stuff>
            {
                new Stuff { Title = "شیر",
                    Inventory=10,
                    MinimumInventory=5, 
                    MaximumInventory=50,
                    Unit="عدد", 
                    CategoryId=categoryId},
                new Stuff { Title = "پنیر",
                    Inventory=20,
                    MinimumInventory=5,
                    MaximumInventory=50,
                    Unit="بسته",
                    CategoryId=categoryId},
                new Stuff { Title = "ماست",
                    Inventory=30,
                    MinimumInventory=5,
                    MaximumInventory=50,
                    Unit="کیلوگرم",
                    CategoryId=categoryId}
            };
        }
    }
}
