using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Invoices.Contracts;
using SuperMarket.Services.Invoices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Invoices
{
    public class InvoiceAppService : InvoiceService
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

        public void Delete(int id, int stuffId, int quantity)
        {
            var invoice = _repository.FindById(id);
            var stuff = _repository.GetStuffById(stuffId);
            stuff.Inventory += quantity;

            _repository.Delete(invoice);
            _unitOfWork.Commit();
        }

        public IList<Invoice> GetAllInvoices()
        {
            return _repository.GetAllInvoices();
        }

        public void Update(int id, UpdateInvoiceDto dto, int stuffId, int quantity)
        {
            var invoice = _repository.FindById(id);

            if (invoice == null)
            {
                throw new InvoiceNotFoundException();
            }

            invoice.Title = dto.Title;
            invoice.Quantity = dto.Quantity;
            invoice.StuffId = dto.StuffId;
            invoice.Buyer = dto.Buyer;
            invoice.Date = dto.Date;
            invoice.Price = dto.Price;

            if (stuffId != dto.StuffId)
            {
                var previousStuff = _repository.GetStuffById(stuffId);
                previousStuff.Inventory += quantity;

                var newStuff = _repository.GetStuffById(dto.StuffId);
                newStuff.Inventory -= dto.Quantity;
            }
            else
            {
                var stuff = _repository.GetStuffById(stuffId);
                stuff.Inventory += quantity;
                stuff.Inventory -= dto.Quantity;
            }

            _unitOfWork.Commit();
        }
    }
}

