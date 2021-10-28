using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer
{
    public class Tbl_DepositTypesMetadata
    {
        [Key]
        public int DepositTypeID { get; set; }
        [Display(Name = "نوع واریز")]
        public string DepositTypeName { get; set; }
    }
    [MetadataType(typeof(Tbl_DepositTypesMetadata))]
    public partial class Tbl_DepositTypes
    {

    }
}
