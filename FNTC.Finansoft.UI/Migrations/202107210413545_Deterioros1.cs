namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deterioros1 : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "DCar.CuentaDeterioroCartera",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CuentaDeterioro = c.String(nullable: false, maxLength: 30),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            
            CreateTable(
                "DCar.DeterioroPar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rango = c.String(nullable: false, maxLength: 10),
                        Desde = c.String(nullable: false, maxLength: 10),
                        Hasta = c.String(nullable: false, maxLength: 10),
                        TipoProvision = c.String(nullable: false, maxLength: 15),
                        PProvision = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "DCar.DeterioroCartera",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdRango = c.Int(nullable: false),
                        Metodo = c.String(nullable: false, maxLength: 10),
                        ValorSuma = c.String(nullable: false, maxLength: 50),
                        FechaGenerada = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DCar.DeterioroPar", t => t.IdRango, cascadeDelete: true)
                .Index(t => t.IdRango);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("DCar.DeterioroCartera", "IdRango", "DCar.DeterioroPar");
            DropIndex("DCar.DeterioroCartera", new[] { "IdRango" });
            DropTable("DCar.DeterioroCartera");
            DropTable("DCar.DeterioroPar");
            DropTable("DCar.CuentaDeterioroCartera");
           
        }
    }
}
