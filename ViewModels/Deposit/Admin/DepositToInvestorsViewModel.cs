using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DepositToInvestorsViewModel
    {
        public List<PlanViewModel> PlansList { get; set; }
        public List<DepositTypeViewModel> DepositTypesList { get; set; }
        [Display(Name = "پرداخت شده")]
        public bool IsPaid { get; set; }
        [Display(Name = "نام طرح")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int Plan_id { get; set; }
        [Display(Name = "نوع واریز")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int DepositType_id { get; set; }
        [Display(Name = "درصد واریز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression("[+-]?([0-9]*[.])?[0-9]+", ErrorMessage = "عدد می بایست طبیعی یا اعشاری باشد مثال 2.5 یا 2")]
        public string StringYieldPercent { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal YieldPercent { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "تاریخ واریز")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DepositDate { get; set; }
        [Display(Name = "تاریخ واریز")]
        public string StringDepositDate { get; set; }
    }
}
