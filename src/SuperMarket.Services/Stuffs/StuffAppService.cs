using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Stuffs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Stuffs
{
    public class StuffAppService:StuffService
    {
        private readonly StuffRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public StuffAppService(
            StuffRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
