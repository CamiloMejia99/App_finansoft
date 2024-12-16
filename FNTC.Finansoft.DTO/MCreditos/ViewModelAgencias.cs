using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelAgencias
    {
        [Key]
        public int codigoagencia { get; set; }
        public string nombreagencia { get; set; }
    }
}
