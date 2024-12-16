using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.ControlCartera;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.Alumno")]
    public partial class Alumno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alumno()
        {
            Seccion = new HashSet<Seccion>();
        }

        [Key]
        public int id_alu { get; set; }

        [Required]
        [StringLength(8)]
        public string dni_alu { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_alu { get; set; }

        [Required]
        [StringLength(100)]
        public string apellidos_alu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seccion> Seccion { get; set; }

        //-------------------MÉTODOS--------------------------------------------------
        public List<Alumno> listar()
        {
            var alumnos = new List<Alumno>();
            string cadena = "SELECT * FROM ALUMNO";
            try
            {
                using (var contenedor = new Model1())
                {
                    alumnos = contenedor.Database.SqlQuery<Alumno>(cadena).ToList();
                }
            }
            catch (Exception)
            {

                //throw;
            }
            return alumnos;

        }
        public Boolean Insertar(string dni, string nom, string ape)
        {
            bool estado = false;
            string cadena = "'" + dni + "',";
            cadena = cadena + "'" + nom + "',";
            cadena = cadena + "'" + ape + "'";
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("INSERT INTO ALUMNO VALUES (" + cadena + ")");
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
        public Alumno un_registro(int id)
        {
            var registro = new Alumno();
            try
            {
                using (var cnx = new Model1())
                {
                    registro = cnx.Alumno.Where(a => a.id_alu == id).Single();
                }
            }
            catch (Exception)
            {

                //throw;
            }
            return registro;
        }
        public Boolean Actualizar(int id, string nom, string ape, string dni)
        {
            bool estado = false;
            string cadena = "dni_alu='" + dni + "', nombre_alu='" + nom + "', apellidos_alu='" + ape + "'";
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("UPDATE ALUMNO SET " + cadena + " WHERE id_alu=" + id);
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
        public Boolean Eliminar(int id)
        {
            bool estado = false;
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("DELETE FROM ALUMNO WHERE id_alu=" + id);
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
    }
}