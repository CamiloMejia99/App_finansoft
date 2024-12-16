using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Personal
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Personal_Id { get; set; }

        [Display(Name = "IDENTIFICACION")]
        public long NIT { get; set; }

        public string PagarePer { get; set; }
    }
}
