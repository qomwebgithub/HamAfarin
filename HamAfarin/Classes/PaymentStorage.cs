using Parbad.Storage.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataLayer;
using ParbadPayment = Parbad.Storage.Abstractions.Models.Payment;
using ParbadTransaction = Parbad.Storage.Abstractions.Models.Transaction;
using Payment = DataLayer.Payment;
using Transaction = DataLayer.Transaction;
using AutoMapper;
using System.Data.Entity;
using System.Collections.Generic;

namespace HamAfarin.Classes
{
    public class PaymentStorage : IStorage
    {
        // In-Memory data
        private HamafarinPaymentDBEntities _paymentDb = new HamafarinPaymentDBEntities();

        public IQueryable<ParbadPayment> Payments => _paymentDb.Payment
            .Select(p => new ParbadPayment
            {
                Id = p.Id,
                TrackingNumber = p.TrackingNumber,
                Amount = p.Amount,
                Token = p.Token,
                TransactionCode = p.TransactionCode,
                GatewayName = p.GatewayName,
                GatewayAccountName = p.GatewayAccountName,
                IsPaid = p.IsPaid,
                IsCompleted = p.IsCompleted,
            });

        public IQueryable<ParbadTransaction> Transactions => _paymentDb.Transaction.Cast<ParbadTransaction>()
            .Select(p => new ParbadTransaction
            {
                Id = p.Id,
                Amount = p.Amount,
                Type = p.Type,
                IsSucceed = p.IsSucceed,
                Message = p.Message,
                AdditionalData = p.AdditionalData,
                PaymentId = p.PaymentId,
            });

        public async Task CreatePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {
            Payment paymentDb = new Payment()
            {
                TrackingNumber = payment.TrackingNumber,
                Amount = payment.Amount,
                Token = payment.Token,
                TransactionCode = payment.TransactionCode,
                GatewayName = payment.GatewayName,
                GatewayAccountName = payment.GatewayAccountName,
                IsPaid = payment.IsPaid,
                IsCompleted = payment.IsCompleted,
            };

            _paymentDb.Payment.Add(paymentDb);
            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdatePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {
            var record = await _paymentDb.Payment.SingleOrDefaultAsync(model => model.Id == payment.Id);

            if (record == null) throw new Exception();

            record.Token = payment.Token;
            record.TrackingNumber = payment.TrackingNumber;
            record.TransactionCode = payment.TransactionCode;

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {
            var record = await _paymentDb.Payment.SingleOrDefaultAsync(model => model.Id == payment.Id);

            if (record == null) throw new Exception();

            _paymentDb.Payment.Remove(record);

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            Transaction transactionDb = new Transaction()
            {
                Amount = transaction.Amount,
                Type = (byte)transaction.Type,
                IsSucceed = transaction.IsSucceed,
                Message = transaction.Message,
                AdditionalData = transaction.AdditionalData,
                PaymentId = transaction.PaymentId,
            };

            _paymentDb.Transaction.Add(transactionDb);

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            var record = await _paymentDb.Transaction.SingleOrDefaultAsync(model => model.Id == transaction.Id);

            if (record == null) throw new Exception();

            record.IsSucceed = transaction.IsSucceed;

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            var record = await _paymentDb.Transaction.SingleOrDefaultAsync(model => model.Id == transaction.Id);

            if (record == null) throw new Exception();

            _paymentDb.Transaction.Remove(record);

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }
    }
}