namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.DescuentosNominaEnvioPlanos")]
    public class EnvioPlano
    {
        [Key]
        public int ID { get; set; }
        public string NombreArchivoPlano { get; set; }
        public string DireccionPlano { get; set; }
        public string Extencion { get; set; }
        public string Comentarios { get; set; }

         
    }
}
