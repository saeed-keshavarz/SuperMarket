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

        public void Add(AddInvoiceDto dto)
        {
            var invoice = new Invoice
            {
                Title = dto.Title,
                Date = dto.Date,
                Buyer = dto.Buyer,
                Quantity = dto.Quantity,
                Price = dto.Price,
                StuffId = dto.StuffId,
            };

            _repository.Add(invoice);

            var stuff = _repository.GetStuffById(dto.StuffId);
            stuff.Inventory -= dto.Quantity;

            _unitOfWork.Commit();

        }

        public IList<Invoice> GetAllInvoices()
        {
            return _repository.GetAllInvoices();
        }

        public Invoice GetById(int id)
        {
            return _repository.FindById(id);
        }

        public void Update(int id, UpdateInvoiceDto dto)
        {
            var invoice = _repository.FindById(id);

            if (invoice == null)
            {
                throw new InvoiceNotFoundException();
            }

            if (invoice.StuffId != dto.StuffId)
            {
                var previousStuff = _repository.GetStuffById(invoice.StuffId);
                previousStuff.Inventory += invoice.Quantity;

                var newStuff = _repository.GetStuffById(dto.StuffId);
                newStuff.Inventory -= dto.Quantity;
            }
            else
            {
                var stuff = _repository.GetStuffById(dto.StuffId);
                stuff.Inventory += invoice.Quantity;
                stuff.Inventory -= dto.Quantity;
            }


            invoice.Title = dto.Title;
            invoice.Quantity = dto.Quantity;
            invoice.StuffId = dto.StuffId;
            invoice.Buyer = dto.Buyer;
            invoice.Date = dto.Date;
            invoice.Price = dto.Price;

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var invoice = _repository.FindById(id);

            if (invoice == null)
            {
                throw new InvoiceNotFoundException();
            }

            var stuff = _repository.GetStuffById(invoice.StuffId);
            stuff.Inventory += invoice.Quantity;

            _repository.Delete(invoice);
            _unitOfWork.Commit();
        }
    }
}

