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
using System.Linq.Expressions;
using Parbad.Storage.Abstractions.Models;

namespace HamAfarin.Classes
{
    public class PaymentStorage : IStorage
    {
        private HamafarinPaymentDBEntities _paymentDb = new HamafarinPaymentDBEntities();

        public IQueryable<ParbadPayment> Payments => _paymentDb.Payment.Select(ToPaymentModel());
        public IQueryable<ParbadTransaction> Transactions => _paymentDb.Transaction.Select(ToTransactionModel());
        
        public async Task CreatePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {

            if (payment == null) throw new ArgumentNullException(nameof(payment));

            var entity = ToPaymentEntity(payment);
            entity.CreatedOn = DateTime.UtcNow;

            _paymentDb.Payment.Add(entity);

            await _paymentDb.SaveChangesAsync(cancellationToken);

            _paymentDb.Entry(entity).State = EntityState.Detached;

            payment.Id = entity.Id;
        }

        public async Task UpdatePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {
            if (payment == null) throw new ArgumentNullException(nameof(payment));

            var record = await _paymentDb.Payment
                .SingleOrDefaultAsync(model => model.Id == payment.Id, cancellationToken);

            if (record == null) throw new InvalidOperationException($"No payment records found in database with id {payment.Id}");

            ToPaymentEntity(payment, record);
            record.UpdatedOn = DateTime.UtcNow;

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePaymentAsync(ParbadPayment payment, CancellationToken cancellationToken = default)
        {

            if (payment == null) throw new ArgumentNullException(nameof(payment));

            var record = await _paymentDb.Payment
                .SingleOrDefaultAsync(model => model.Id == payment.Id, cancellationToken);

            if (record == null) throw new InvalidOperationException($"No payment records found in database with id {payment.Id}");

            _paymentDb.Payment.Remove(record);

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var entity = ToTransactionEntity(transaction);
            entity.CreatedOn = DateTime.UtcNow;

            _paymentDb.Transaction.Add(entity);

            await _paymentDb.SaveChangesAsync(cancellationToken);

            transaction.Id = entity.Id;
        }

        public async Task UpdateTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var record = await _paymentDb.Transaction
                .SingleOrDefaultAsync(model => model.Id == transaction.Id, cancellationToken);

            if (record == null) throw new InvalidOperationException($"No transaction records found in database with id {transaction.Id}");

            ToTransactionEntity(transaction, record);
            record.UpdatedOn = DateTime.UtcNow;

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTransactionAsync(ParbadTransaction transaction, CancellationToken cancellationToken = default)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var record = await _paymentDb.Transaction
                .SingleOrDefaultAsync(model => model.Id == transaction.Id, cancellationToken);

            if (record == null) throw new InvalidOperationException($"No transaction records found in database with id {transaction.Id}");

            _paymentDb.Transaction.Remove(record);

            await _paymentDb.SaveChangesAsync(cancellationToken);
        }

        private Expression<Func<Payment, ParbadPayment>> ToPaymentModel()
        {
            return entity => new ParbadPayment
            {
                Id = entity.Id,
                TrackingNumber = entity.TrackingNumber,
                Amount = entity.Amount,
                Token = entity.Token,
                TransactionCode = entity.TransactionCode,
                GatewayName = entity.GatewayName,
                GatewayAccountName = entity.GatewayAccountName,
                IsCompleted = entity.IsCompleted,
                IsPaid = entity.IsPaid
            };
        }

        public Expression<Func<Transaction, ParbadTransaction>> ToTransactionModel()
        {
            return entity => new ParbadTransaction
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Type = (TransactionType)entity.Type,
                IsSucceed = entity.IsSucceed,
                Message = entity.Message,
                AdditionalData = entity.AdditionalData,
                PaymentId = entity.PaymentId
            };
        }

        private Payment ToPaymentEntity(ParbadPayment model)
        {
            return new Payment
            {
                TrackingNumber = model.TrackingNumber,
                Amount = model.Amount,
                Token = model.Token,
                TransactionCode = model.TransactionCode,
                GatewayName = model.GatewayName,
                GatewayAccountName = model.GatewayAccountName,
                IsCompleted = model.IsCompleted,
                IsPaid = model.IsPaid
            };
        }

        private Transaction ToTransactionEntity(ParbadTransaction model)
        {
            return new Transaction
            {
                Amount = model.Amount,
                Type = (byte)model.Type,
                IsSucceed = model.IsSucceed,
                Message = model.Message,
                AdditionalData = model.AdditionalData,
                PaymentId = model.PaymentId
            };
        }

        private void ToPaymentEntity(ParbadPayment model, Payment entity)
        {
            entity.TrackingNumber = model.TrackingNumber;
            entity.Amount = model.Amount;
            entity.Token = model.Token;
            entity.TransactionCode = model.TransactionCode;
            entity.GatewayName = model.GatewayName;
            entity.GatewayAccountName = model.GatewayAccountName;
            entity.IsCompleted = model.IsCompleted;
            entity.IsPaid = model.IsPaid;
        }
        private void ToTransactionEntity(ParbadTransaction model, Transaction entity)
        {
            entity.Amount = model.Amount;
            entity.Type = (byte)model.Type;
            entity.IsSucceed = model.IsSucceed;
            entity.Message = model.Message;
            entity.AdditionalData = model.AdditionalData;
        }
    }

}