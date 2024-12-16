using System;
using System.Collections.Generic;



namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaArchivoPlanos")]
    public class ArchivoPlano
    {
        [Key]

        public int ID { get; set; }

        [ForeignKey("ClaseDePlanos1")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Clase de Plano")]
        public int CLASEPLANO { get; set; }


        [ForeignKey("TipoDeCampo")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo de Campo")]
        public int TIPCAMPO { get; set; }


        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Concepto")]
        public string CONCEPTO { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]

        [StringLength(255)]


        [Display(Name = "Tipo de Dato")]
        public string TIPDATO { get; set; }

        [StringLength(255)]

        [Required(ErrorMessage = "Este campo es obligatorio")]

        [Display(Name = "Longitud")]

        public string LONGITUD { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Alineacion")]
        public string ALINEACION { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Relleno")]
        public string RELLENO { get; set; }

        [StringLength(255)]
        [Display(Name = "Valor Predetermimnado")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VALPREDETERINADO { get; set; }



        [Display(Name = "Digite Valor")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int DIGVALOR { get; set; }


        [Display(Name = "Numero de Decimales")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int NUMDECIMALES { get; set; }


        [Display(Name = "Orden")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int ORDEN { get; set; }

        public IList<ArchivoPlano> ToList()
        {
            throw new NotImplementedException();
        }

        //ESTO ES LA LLEVE FORANEA



        public virtual ClaseDePlano ClaseDePlanos1 { get; set; }

        public virtual TipoDeCampo TipoDeCampo { get; set; }


    }
}