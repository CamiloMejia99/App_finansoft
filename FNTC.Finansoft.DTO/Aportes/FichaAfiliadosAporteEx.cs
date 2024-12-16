
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNTC.Finansoft.Accounting.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.Accounting.DTO.Fichas;

namespace FNTC.Finansoft.Accounting.DTO.Aportes
{
    [Table("apo.AfiliadosAporteEx")]
    public partial class FichaAfiliadosAporteEx
    {

        [Key]
        public int IdAfiliadosAporteEx { get; set; }
        public string NumeroCuenta { get; set; }
        [ForeignKey("Configuracion2ExFK")]
        public int IdConfiguracion { get; set; }
        public string idPersona { get; set; }
        public long totalAportesEx { get; set; }
        public DateTime FechaApertura { get; set; }
        public string asesor { get; set; }
        public bool Estado { get; set; }

        public virtual Configuracion2Ex Configuracion2ExFK { get; set; }

        //-------------------MÉTODOS--------------------------------------------------
        public string Insertar(FichaAfiliadosAporteEx fichaAfiliadoEx, int Configuracion, string Abreviado, string useractual, string consulta,string consultaTercero)
        {
            string estado = "";
            string DataBB = consulta;
            string IdUsuario = fichaAfiliadoEx.idPersona;
            using (var contextoFinansoft = new AccountingContext())

                if (IdUsuario == null)
                {
                    estado = "SE DEBE SELECCIONAR UN TERCERO PARA EL REGISTRO";
                }
                else
                {

                    if (DataBB == "UsuarioNoExistente")
                    {
                        if (consultaTercero == "TerceroNoExistente") 
                        {
                            estado = "EL USUARIO NO EXISTE";
                        }
                        else
                        {
                            fichaAfiliadoEx.NumeroCuenta = Abreviado + fichaAfiliadoEx.idPersona;
                            fichaAfiliadoEx.IdConfiguracion = Configuracion;
                            fichaAfiliadoEx.idPersona = fichaAfiliadoEx.idPersona;
                            fichaAfiliadoEx.totalAportesEx = 0;
                            fichaAfiliadoEx.FechaApertura = DateTime.Now;
                            fichaAfiliadoEx.asesor = useractual;
                            fichaAfiliadoEx.Estado = fichaAfiliadoEx.Estado;
                            contextoFinansoft.FichaAfiliadosAporteEx.Add(fichaAfiliadoEx);
                            contextoFinansoft.SaveChanges();
                            estado = "Registrado Correctamente";
                        }
                        

                    }
                    else
                    {
                        estado = "EL USUARIO YA SE ENCUENTRA AFILIADO A APORTES EXTRAORDINARIOS";
                    }

                }


            return estado;
        }
        public Boolean Eliminar(int id)
        {
            bool estado = false;

            try
            {
                using (var contextoFinansoft = new AccountingContext())
                {
                    int r = contextoFinansoft.Database.ExecuteSqlCommand("DELETE FROM apo.AfiliadosAporteEx WHERE IdAfiliadosAporteEx=" + id);
                    if (r == 1)
                    {
                        estado = true;
                    }
                }
            }
            catch (Exception)
            {
                estado = false;
                //throw;
            }
            return estado;
        }
        public string Abreviado()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var config = "";
                var Configuracion = contextoFinansoft.Configuracion2Ex.Where(t => t.estado == true).FirstOrDefault();
                if (Configuracion != null)
                {
                    config = Configuracion.nombreAbreviatura;
                }

                return config;
            }
        }


    }

}

