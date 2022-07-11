using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HamAfarin.Classes
{
    public class WalletService
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();


        public long GetUserBalance(int userId)
        {
            List<Tbl_Wallet> wallet = db.Tbl_Wallet.Where(w => w.User_id == userId).ToList();

            if (wallet == null)
                return 0;

            //1 = واریز
            long deposit = wallet.Where(w => w.Type_id == 1).Select(w => w.Amount).Sum();

            //2 = برداشت
            long withdrawal = wallet.Where(w => w.Type_id == 2).Select(w => w.Amount).Sum();

            long balance = deposit - withdrawal;

            if (balance < 0)
            {
                //Todo: Send SMS to Owner If Balace goes negative
                //

                //show 0 if balance goes negative
                return 0;
            }

            return balance;
        }

        public void AddTransaction(int userId, long amount, string description)
        {
            var transaction = db.Database.BeginTransaction();

            Tbl_Wallet wallet = new Tbl_Wallet()
            {
                User_id = userId,
                //1 = واریز
                //2 = برداشت
                Type_id = amount >= 0 ? 1 : 2,
                Amount = Math.Abs(amount),
                Description = description,
                CreateDate = DateTime.Now
            };


            db.Tbl_Wallet.Add(wallet);
            db.SaveChanges();

            Tbl_Users qUser = db.Tbl_Users.Find(userId);

            qUser.WalletBalance = GetUserBalance(qUser.UserID);
            db.SaveChanges();

            transaction.Commit();
        }
    }
}