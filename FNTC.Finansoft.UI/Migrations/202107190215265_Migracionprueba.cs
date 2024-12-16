namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracionprueba : DbMigration
    {
        public override void Up()
        {
                       
            CreateTable(
                "Gcar.CompromisoCartera",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdGestion = c.Int(nullable: false),
                        ObservacionCompromiso = c.String(nullable: false),
                        FechaCompromiso = c.DateTime(nullable: false),
                        ValorCompromiso = c.String(nullable: false),
                        TipoCompromiso = c.String(),
                        ValidacionCompromiso = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Gcar.GCgestion", t => t.IdGestion, cascadeDelete: true)
                .Index(t => t.IdGestion);
            
            CreateTable(
                "Gcar.GCgestion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        idAsociado = c.Int(nullable: false),
                        ClaseGestion = c.String(nullable: false, maxLength: 20),
                        FechaGestion = c.DateTime(nullable: false),
                        RespuestaGestion = c.String(nullable: false),
                        ResOpcionalGestion = c.String(maxLength: 20),
                        ContactoGestion = c.String(nullable: false, maxLength: 200),
                        GestionVerificada = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
            
        }
        
        public override void Down()
        {
           DropForeignKey("Gcar.CompromisoCartera", "IdGestion", "Gcar.GCgestion");
           DropIndex("Gcar.CompromisoCartera", new[] { "IdGestion" });
           DropTable("Gcar.GCgestion");
            DropTable("Gcar.CompromisoCartera");
           
        }
    }
}
