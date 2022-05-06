using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Invoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Invoices
{
    public class InvoiceAppService:InvoiceService
    {
        private readonly InvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public InvoiceAppService(
            InvoiceRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddInvoiceDto dto, int stuffId)
        {
            var invoice = new Invoice
            {
                Title = dto.Title,
                Date = dto.Date,
                Buyer = dto.Buyer,
                Quantity = dto.Quantity,
                Price = dto.Price,
                StuffId = stuffId,
            };

            _repository.Add(invoice);

            var stuff = _repository.GetStuffById(stuffId);
            stuff.Inventory -= dto.Quantity;

            _unitOfWork.Commit();

        }
    }
}
