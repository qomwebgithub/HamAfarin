using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Tbl_RolesMetadata
    {
        [Key]
        public int RoleID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string RoleSystemName { get; set; }

        [Display(Name = "نقش")]
        public string RoleDisplayTitle { get; set; }
    }

    [MetadataType(typeof(Tbl_RolesMetadata))]
    public partial class Tbl_Roles
    {

    }
}
