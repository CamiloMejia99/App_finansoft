namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class infoadicionalterceros2021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ter.contrato",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipoContrato = c.String(nullable: false, maxLength: 30),
                        Detalle = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ter.estrato",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Estrato = c.String(nullable: false, maxLength: 20),
                        Detalle = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ter.InfoTerceroAdicional",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NitTercero = c.String(nullable: false, maxLength: 20),
                        IdEstarto = c.Int(nullable: false),
                        IdContrato = c.Int(nullable: false),
                        IdNivelEstudio = c.Int(nullable: false),
                        PersonasCargo = c.Int(nullable: false),
                        Ocupacion = c.String(maxLength: 50),
                        Detalle = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ter.contrato", t => t.IdContrato, cascadeDelete: true)
                .ForeignKey("ter.estrato", t => t.IdEstarto, cascadeDelete: true)
                .ForeignKey("ter.nivelestudio", t => t.IdNivelEstudio, cascadeDelete: true)
                .ForeignKey("ter.Terceros", t => t.NitTercero, cascadeDelete: true)
                .Index(t => t.NitTercero)
                .Index(t => t.IdEstarto)
                .Index(t => t.IdContrato)
                .Index(t => t.IdNivelEstudio);
            
            CreateTable(
                "ter.nivelestudio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nestudio = c.String(nullable: false, maxLength: 25),
                        Detalle = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ter.InfoTerceroAdicional", "NitTercero", "ter.Terceros");
            DropForeignKey("ter.InfoTerceroAdicional", "IdNivelEstudio", "ter.nivelestudio");
            DropForeignKey("ter.InfoTerceroAdicional", "IdEstarto", "ter.estrato");
            DropForeignKey("ter.InfoTerceroAdicional", "IdContrato", "ter.contrato");
            DropIndex("ter.InfoTerceroAdicional", new[] { "IdNivelEstudio" });
            DropIndex("ter.InfoTerceroAdicional", new[] { "IdContrato" });
            DropIndex("ter.InfoTerceroAdicional", new[] { "IdEstarto" });
            DropIndex("ter.InfoTerceroAdicional", new[] { "NitTercero" });
            DropTable("ter.nivelestudio");
            DropTable("ter.InfoTerceroAdicional");
            DropTable("ter.estrato");
            DropTable("ter.contrato");
        }
    }
}
