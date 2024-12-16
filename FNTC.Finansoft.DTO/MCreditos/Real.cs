using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Real
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Real_Id { get; set; }

        [Display(Name = "Valor")]
        public long Real_Valor { get; set; }

        public string PagareId { get; set; }
    }
}
