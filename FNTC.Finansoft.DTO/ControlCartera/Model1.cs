﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using FNTC.Finansoft.Accounting.DTO.ControlCartera;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=AccContext")
        {
        }

        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>()
                .Property(e => e.dni_alu)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.nombre_alu)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.apellidos_alu)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.Seccion)
                .WithMany(e => e.Alumno)
                .Map(m => m.ToTable("Detalle_asig_alumno_seccion").MapLeftKey("id_alu").MapRightKey("id_sec"));

            modelBuilder.Entity<Curso>()
                .Property(e => e.descripcion_cur)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Seccion)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);
        }
    }
}
