using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels;
using DataLayer;
using Hamafarin;
using System.Globalization;

namespace HamAfarin
{
    public class PlanService
    {
        /// <summary>
        /// لیست همه کامنتهای یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <param name="db">اتصال بانک</param>
        /// <returns>لیست نظر ها</returns>
        public List<PlanCommentItemViewModel> GetCommentsOfPlan(HamAfarinDBEntities db, int id)
        {
            //همه نظرات یک طرح
            List<Tbl_CommentPlan> qAllListComments = db.Tbl_CommentPlan.Where(c => c.BusinessPlan_id == id && c.IsActive && c.IsDeleted == false).ToList();
            // نظرات اصلی یک طرح
            List<Tbl_CommentPlan> qListComments = qAllListComments.Where(c => c.Parent_id == null).ToList();
            //ساخت لیست خروجی
            List<PlanCommentItemViewModel> listComments = new List<PlanCommentItemViewModel>();
            Tbl_UserProfiles qProfile = new Tbl_UserProfiles();
            string UserName = "بدون نام";
            foreach (var item in qListComments)
            {
                // لیست نظرات بچه
                List<Tbl_CommentPlan> qChildListComments = qAllListComments.Where(c => c.Parent_id == item.CommentID).ToList();
                List<PlanCommentItemViewModel> listChildComments = new List<PlanCommentItemViewModel>();
                foreach (var item2 in qChildListComments)
                {
                    qProfile = new Tbl_UserProfiles();
                    UserName = "بدون نام";
                    qProfile = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == item2.User_id && u.IsDeleted == false);
                    if (qProfile != null)
                    {
                        UserName = qProfile.FirstName;
                    }
                    if (item2.User_id == 1)
                    {
                        UserName = "مدیر سایت";
                    }
                    listChildComments.Add(new PlanCommentItemViewModel()
                    {
                        CommentID = item2.CommentID,
                        CommentText = item2.CommentText,
                        Parent_id = item2.Parent_id,
                        User_id = item2.User_id,
                        UserName = UserName,
                        CreateDate = item2.CreateDate
                    });
                }
                qProfile = new Tbl_UserProfiles();
                UserName = "بدون نام";
                qProfile = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == item.User_id && u.IsDeleted == false);
                if (qProfile != null)
                {
                    UserName = qProfile.FirstName;
                }
                if (item.User_id == 1)
                {
                    UserName = "مدیر سایت";
                }
                listComments.Add(new PlanCommentItemViewModel()
                {
                    CommentID = item.CommentID,
                    CommentText = item.CommentText,
                    Parent_id = item.Parent_id,
                    User_id = item.User_id,
                    UserName = UserName,
                    CreateDate = item.CreateDate,
                    Tbl_CommentPlan1 = listChildComments
                });
            }
            return listComments;
        }
        /// <summary>
        /// کل مبلغ سرمایه گذاری شده در این طرح برای شخص
        /// </summary>
        /// <param name="db">بانک</param>
        /// <param name="id">شناسه طرح</param>
        /// <param name="myUserId">شناسه کاربر</param>
        /// <returns></returns>
        internal long TotalUserInvestmentForPlan(HamAfarinDBEntities db, int id, int myUserId)
        {
            long? result = db.Tbl_BusinessPlanPayment
                .Where(p =>
                    p.IsDelete == false &&
                    p.BusinessPlan_id == id &&
                   // p.IsConfirmedFromAdmin == true &&
                    p.IsPaid == true &&
                    p.PaymentUser_id == myUserId)
                .Sum(s => s.PaymentPrice);

            if (result == null) return 0;
            return (long)result;
        }

        /// <summary>
        /// لیست همه سوالهای یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <param name="db">اتصال بانک</param>
        /// <returns>لیست سوالها</returns>
        public List<PlanQuestionItemViewModel> GetQuestionsOfPlan(HamAfarinDBEntities db, int id)
        {
            //همه نظرات یک طرح
            List<Tbl_BusinessPlanQuestion> qAllListQuestions = db.Tbl_BusinessPlanQuestion.Where(c => c.BusinessPlan_id == id && c.IsActive && c.IsDeleted == false).ToList();
            // نظرات اصلی یک طرح
            List<Tbl_BusinessPlanQuestion> qListQuestions = qAllListQuestions.Where(c => c.Parent_id == null).ToList();
            //ساخت لیست خروجی
            List<PlanQuestionItemViewModel> listQuestions = new List<PlanQuestionItemViewModel>();
            Tbl_UserProfiles qProfile = new Tbl_UserProfiles();
            string UserName = "بدون نام";
            foreach (var item in qListQuestions)
            {

                // لیست نظرات بچه
                List<Tbl_BusinessPlanQuestion> qChildListQuestions = qAllListQuestions.Where(c => c.Parent_id == item.QuestionID).ToList();
                List<PlanQuestionItemViewModel> listChildQuestions = new List<PlanQuestionItemViewModel>();
                foreach (var item2 in qChildListQuestions)
                {
                    qProfile = new Tbl_UserProfiles();
                    UserName = "بدون نام";
                    qProfile = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == item2.User_id && u.IsDeleted == false);
                    if (qProfile != null)
                    {
                        UserName = qProfile.FirstName;
                    }
                    if (item2.User_id == 1)
                    {
                        UserName = "مدیر سایت";
                    }
                    listChildQuestions.Add(new PlanQuestionItemViewModel()
                    {
                        QuestionID = item2.QuestionID,
                        QuestionText = item2.QuestionText,
                        Parent_id = item2.Parent_id,
                        User_id = item2.User_id,
                        UserName = UserName,
                        CreateDate = item2.CreateDate
                    });
                }
                qProfile = new Tbl_UserProfiles();
                UserName = "بدون نام";
                qProfile = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == item.User_id && u.IsDeleted == false);
                if (qProfile != null)
                {
                    UserName = qProfile.FirstName;
                }
                if (item.User_id == 1)
                {
                    UserName = "مدیر سایت";
                }
                listQuestions.Add(new PlanQuestionItemViewModel()
                {
                    QuestionID = item.QuestionID,
                    QuestionText = item.QuestionText,
                    Parent_id = item.Parent_id,
                    User_id = item.User_id,
                    UserName = UserName,
                    CreateDate = item.CreateDate,
                    Tbl_BusinessPlanQuestion1 = listChildQuestions
                });
            }
            return listQuestions;
        }

        /// <summary>
        /// تبدیل اعداد فارسی به انگلیسی
        /// </summary>
        /// <param name="persianNumber">عدد فارسی</param>
        /// <returns>عدد انگلیسی</returns>
        public string GetEnglishNumber(string persianNumber)
        {
            string englishNumber = "";
            foreach (char ch in persianNumber)
            {
                englishNumber += char.GetNumericValue(ch);
            }
            return englishNumber;
        }
        /// <summary>
        /// محاسبه درصد بین دو عدد
        /// </summary>
        /// <param name="totalPrice">کل مبلغ</param>
        /// <param name="raisedPrice">مبلغ بدست آمده</param>
        /// <returns>درصد به عدد صحیح</returns>
        public int GetPercentage(long totalPrice, long raisedPrice)
        {
            float fPercentage = (raisedPrice / (float)totalPrice) * 100;
            int intPercentage = (int)fPercentage;
            return intPercentage;
        }

        /// <summary>
        /// محاسبه میزان سرمایه گذاری ها در طرح
        /// </summary>
        /// <param name="db">دیتابیس</param>
        /// <param name="id">شناسه طرح</param>
        /// <returns>مبلغ کل سرمایه گذاری شده</returns>
        public long GetRaisedPrice(HamAfarinDBEntities db, int id)
        {
            List<Tbl_BusinessPlanPayment> qList = GetPlanSubmittedPaid(db, id);
            long? total = qList.Where(t => t.BusinessPlan_id == id).Sum(i => i.PaymentPrice);
            return total.Value;
        }
        /// <summary>
        /// محاسبه زمان باقیمانده طرح
        /// </summary>
        /// <param name="endDate">ایتم طرح</param>
        /// <returns>زمان باقیمانده یا منقضی شده</returns>
        public int CalculateRemainDay(DateTime? endDate)
        {
            DateTime dateTimeNow = DateTime.Now;

            if (endDate == null)
                return 0;

            TimeSpan difference = endDate.Value - dateTimeNow;
            if (difference.TotalSeconds <= 0)
                return -1;

            int finalBetweenDay = difference.Days;

            return finalBetweenDay;
        }

        public bool IsAcceptInvestmentPlan(HamAfarinDBEntities db, int id)
        {
            Tbl_BussinessPlans qBussinessPlan = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id && p.IsActive && p.IsDeleted == false);
            if (qBussinessPlan != null)
            {
                // مبلغ سرمایه گذاری شده
                long intRaisedPrice = GetRaisedPrice(db, qBussinessPlan.BussinessPlanID);
                //اگر مبلغ سرمایه گذاری شده برای طرح کامل شده باشد
                if (intRaisedPrice >= long.Parse(qBussinessPlan.AmountRequiredRoRaiseCapital))
                {
                    if (qBussinessPlan.IsOverflowInvestment == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public DateTime GetPersianDateTime(DateTime dateTimeParam)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(
                                                persianCalendar.GetYear(dateTimeParam),
                                                persianCalendar.GetMonth(dateTimeParam),
                                                persianCalendar.GetDayOfMonth(dateTimeParam),
                                                persianCalendar.GetHour(dateTimeParam),
                                                persianCalendar.GetMinute(dateTimeParam),
                                                persianCalendar.GetSecond(dateTimeParam),
                                                (int)persianCalendar.GetMilliseconds(dateTimeParam),
                                                persianCalendar.GetEra(dateTimeParam)
                                              );
        }

        /// <summary>
        /// لیست طرح های فعال
        /// </summary>
        /// <returns>لیست طرح های فعال</returns>
        public List<Tbl_BussinessPlans> ActivePlans(HamAfarinDBEntities db)
        {
            return db.Tbl_BussinessPlans.Where(b => b.IsActive && b.IsDeleted == false).ToList();
        }
        /// <summary>
        /// دریافت  پرداختی های تایید شده
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <returns>لیست پرداختی های تایید شده طرح</returns>
        public List<Tbl_BusinessPlanPayment> GetPlanSubmittedPaid(HamAfarinDBEntities db, int id)
        {
            return db.Tbl_BusinessPlanPayment.Where(p => p.BusinessPlan_id == id && p.IsConfirmedFromAdmin && p.IsPaid && p.IsDelete == false).ToList();
        }
        public long GetPlanSubmittedPaidByUserId(HamAfarinDBEntities db, int id, int userId)
        {
            return db.Tbl_BusinessPlanPayment.Where(p => p.IsDelete == false && p.BusinessPlan_id == id && p.CreateUser_id == userId && p.IsConfirmedFromAdmin && p.IsPaid).Sum(s => s.PaymentPrice).Value;
        }
        /// <summary>
        /// دریافت تعداد سرمایه گذارهای یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح</param>
        /// <returns>تعداد سرمایه گذار یک طرح</returns>
        public int GetPlanInvestorCount(HamAfarinDBEntities db, int id)
        {
            return GetPlanSubmittedPaid(db, id).Select(p => p.PaymentUser_id).Distinct().Count(); ;
        }

        public int GetPercentageInvestmentPlan(string AmountRequiredRoRaiseCapital, long intRaisedPrice)
        {
            int qPercentageComplate = GetPercentage(long.Parse(AmountRequiredRoRaiseCapital), intRaisedPrice);
            return qPercentageComplate;
        }

        /// <summary>
        /// کنترل معتبر بودن مبلغ
        /// </summary>
        /// <param name="db">بانک اطلاعاتی</param>
        /// <param name="paymentOnlineViewModel">مدل پرداخت</param>
        /// <returns>معتبر بودن یا نبودن و متن خروجی</returns>
        public PaymentPriceValidation PaymentPriceValidation(HamAfarinDBEntities db, int? businessPlanID, long? paymentPrice, int? userId, bool isLegal = false)
        {
            PaymentPriceValidation retValue = new PaymentPriceValidation();
            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == businessPlanID && p.IsActive && p.IsDeleted == false);
            long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
            //چک کردن حداکثر مبلغ

            // سرمایه گذاری من در این طرح
            long userTotalInvestment = new PlanService().TotalUserInvestmentForPlan(db, businessPlanID.Value, userId.Value);

            if (paymentPrice + userTotalInvestment > MaximumInvestment && isLegal == false)
            {
                retValue.Validation = false;
                retValue.Error = "مبلغ وارد شده بیشتر از حداکثر میباشد";
                return retValue;
            }
            //چک کردن حداقل مبلغ
            if (paymentPrice < long.Parse(qBussinessPlans.MinimumAmountInvest))
            {
                retValue.Validation = false;
                retValue.Error = "مبلغ وارد شده کمتر از حداقل میباشد";
                return retValue;
            }
            //چک کردن ضریب مبلغ
            long a = (paymentPrice % long.Parse(qBussinessPlans.MinimumAmountInvest)).Value;
            if (paymentPrice % long.Parse(qBussinessPlans.MinimumAmountInvest) != 0)
            {
                retValue.Validation = false;
                retValue.Error = "مبلغ وارد شده ضریب درستی ندارد";
                return retValue;
            }
            //چک کردن بیش از صد در صد سرمایه گذاری
            if (!qBussinessPlans.IsOverflowInvestment)
            {
                long totalPrice = GetRaisedPrice(db, qBussinessPlans.BussinessPlanID) + paymentPrice.Value;
                if (totalPrice > long.Parse(qBussinessPlans.AmountRequiredRoRaiseCapital))
                {
                    retValue.Validation = false;
                    retValue.Error = "امکان سرمایه گذاری بیشتر از ۱۰۰ درصد وجود ندارد";
                    return retValue;
                }
            }
            retValue.Validation = true;
            return retValue;
        }
    }
}