using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("Seccion")]
    public partial class Seccion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Seccion()
        {
            Alumno = new HashSet<Alumno>();
        }

        [Key]
        public int id_sec { get; set; }

        public int aula_sec { get; set; }

        public int id_cur { get; set; }

        public bool estado_sec { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_registro_sec { get; set; }

        public virtual Curso Curso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alumno> Alumno { get; set; }

        //--------Métodos----------

        public List<Seccion> listar()
        {
            var seccion = new List<Seccion>();
            string cadena = "SELECT * FROM SECCION";
            try
            {
                using (var contenedor = new Model1())
                {
                    seccion = contenedor.Database.SqlQuery<Seccion>(cadena).ToList();
                }
            }
            catch (Exception)
            {

                //throw;
            }
            return seccion;
        }

        public Boolean Insertar(string nu, bool es, string fe, int cu, List<string> indices)
        {
            bool estado = false;
            string cadena = "'" + nu + "',";
            cadena = cadena + "'" + cu + "',";
            cadena = cadena + "'" + es + "',";
            cadena = cadena + "'" + fe + "'";
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("INSERT INTO Seccion VALUES(" + cadena + ")");
                    string codigo_libro = "(SELECT MAX(id_sec) FROM Seccion)";
                    if (!indices.Equals(null))
                    {
                        foreach (var i in indices)
                        {
                            cnx.Database.ExecuteSqlCommand("INSERT INTO Detalle_asig_alumno_seccion VALUES(" +
                                codigo_libro + "," + i + ")");
                        }
                    }
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

        // Capturar un solo registro (fila) --> ingresando su id
        public Seccion un_registro(int id)
        {
            var seccion = new Seccion();
            try
            {
                using (var cnx = new Model1())
                {
                    seccion = cnx.Seccion.Where(r => r.id_sec == id).Single();
                }
            }
            catch (Exception)
            {
                //throw;
            }
            return seccion;
        }

        // Actualizar Seccion
        public Boolean Actualizar(int id, string nu, bool es, string fe, int cu, List<string> indices)
        {
            bool estado = false;
            string cadena = "aula_sec='" + nu + "',";
            cadena = cadena + "id_cur='" + cu + "',";
            cadena = cadena + "estado_sec='" + es + "',";
            cadena = cadena + "fecha_registro_sec='" + fe + "'";
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("UPDATE Seccion SET " + cadena + " WHERE id_sec=" + id);
                    //----------------------------------
                    if (!indices.Equals(null))
                    {
                        cnx.Database.ExecuteSqlCommand("DELETE FROM Detalle_asig_alumno_seccion WHERE id_sec=" + id);
                        foreach (var i in indices)
                        {
                            cnx.Database.ExecuteSqlCommand("INSERT INTO Detalle_asig_alumno_seccion VALUES(" +
                                id + "," + i + ")");
                        }
                    }
                    //-------------------------
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


        //-------------ELIMINAR SECCION-------------------
        public Boolean Eliminar(int id)
        {
            bool estado = false;
            try
            {
                using (var cnx = new Model1())
                {
                    int r = cnx.Database.ExecuteSqlCommand("DELETE FROM Seccion WHERE id_sec=" + id);
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

        //Detalle actualizar Seccion

        public List<Alumno> det_sec(string id_sec)
        {
            var alumnos = new List<Alumno>();
            string cadena = "SELECT a.* FROM detalle_asig_alumno_seccion d INNER JOIN "
                + "Alumno a ON d.id_alu=a.id_alu WHERE d.id_sec=" + id_sec;
            try
            {
                using (var contenedor = new Model1())
                {
                    alumnos = contenedor.Database.SqlQuery<Alumno>(cadena).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return alumnos;
        }

        //-------------------------------- Detalle de seccion
        public List<Alumno> detalle_seccion(string id_sec)
        {
            var alumnos = new List<Alumno>();
            string cadena = "SELECT a.* FROM Detalle_asig_alumno_seccion d INNER JOIN "
            + "Alumno a ON d.id_alu=a.id_alu WHERE d.id_sec=" + id_sec;
            try
            {
                using (var contenedor = new Model1())
                {
                    alumnos = contenedor.Database.SqlQuery<Alumno>(cadena).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return alumnos;
        }


        // Busqueda de Alumno
        public List<Alumno> bus_atr(string dato_bus)
        {
            var autores = new List<Alumno>();
            string cadena = "SELECT * FROM Alumno WHERE id_alu LIKE '%" + dato_bus + "%'";
            try
            {
                using (var contenedor = new Model1())
                {
                    autores = contenedor.Database.SqlQuery<Alumno>(cadena).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return autores;
        }

        // Busqueda de Seccion ----------------------------------------
        public List<Seccion> bus_sec(string tipo_bus, string dato_bus)
        {
            var lista = new List<Seccion>();
            string cadena = "SELECT * FROM Seccion WHERE " + tipo_bus + " LIKE '%"
                + dato_bus + "%'";
            try
            {
                using (var contenedor = new Model1())
                {
                    lista = contenedor.Database.SqlQuery<Seccion>(cadena).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lista;
        }
    }
}
