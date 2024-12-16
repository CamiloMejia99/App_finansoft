using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{

    [Table("aso.Asociados")]
    public class Asociado
    {
        [Key]
        public int Id { get; set; }

        public int Estado { get; set; }

        [Required]
        public string Documento_Numero { get; set; }


        [Required]
        [Display(Prompt = "Tipo Documento", Description = "descripcion", AutoGenerateFilter = true)]
        public int TipoIdentificacion { get; set; }

        [Required]
        public int Documento_Fecha_Expedicion { get; set; }

        [Required]
        [Display(Name = "Pais Expedicion")]
        public int Documento_PaisExpedicion { get; set; }


        #region Pendientes
        public int DeptoExpedicion { get; set; }

        public int Id_LExpedicion { get; set; }


        public string Primer_Nom { get; set; }
        public string Segundo_Nom { get; set; }
        public string Primer_Ape { get; set; }
        public string Segundo_Ape { get; set; }
        public bool Genero { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }

        public int Id_EstadoCivil { get; set; }


        public int PaisResidencia { get; set; }

        public int DeptoResidencia { get; set; }
        public int Id_Residencia { get; set; }


        public string Direccion { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Telefono { get; set; }


        public int ext { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Celular { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        public int Id_TipoEstudio { get; set; }



        public int Id_Profesion { get; set; }



        public string Cuenta { get; set; }

        public int Id_Banco { get; set; }


        public int Id_TipoVivienda { get; set; }



        public int Id_Estrato { get; set; }


        public int Id_Deduccion { get; set; }


        public int Id_EmpresaPaga { get; set; }


        public int Id_Agencia { get; set; }
        public int Id_Dependencia { get; set; }

        public int Id_Contrato { get; set; }



        public DateTime Fecha_Inicio_Emp { get; set; }
        public DateTime Fecha_Fin { get; set; }

        public int Id_Cargo { get; set; }


        public int Salario { get; set; }

        public int Id_CIIU { get; set; }


        public DateTime Fecha_Inicio_Aso { get; set; }
        public string Acta { get; set; }

        #endregion



    }



}