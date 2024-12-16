using FNTC.Finansoft.Accounting.DTO.Email;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("dbo.RolesUsuarioFC")]
    public class FCRolesUsuario
    {
        [Key]
        public int Id_Permisos { get; set; }

        [ForeignKey("AspNetUsers")]
        [Display(Name = "Usuario")]
        public string NIT_Usuario { get; set; }


        [Display(Name = "Rol Operario")]
        public string Rol_Operario { get; set; }

        [Display(Name = "Rol Analista")]
        public string Rol_Analista { get; set; }

        [Display(Name = "Rol Ente")]
        public string Rol_Ente { get; set; }

        [ForeignKey("Dependencias")]
        public int IdDependencia { get; set; }
        [Display(Name = "Rol Informativo")]
        public string Rol_Informativo { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual FCDependencias Dependencias { get; set; }

    }
}