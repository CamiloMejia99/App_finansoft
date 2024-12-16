namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabafinancieraterceros : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "ter.InfoTerceroAdicional", name: "IdEstarto", newName: "IdEstrato");
            RenameIndex(table: "ter.InfoTerceroAdicional", name: "IX_IdEstarto", newName: "IX_IdEstrato");
            CreateTable(
                "ter.InfoTerceroFinanciera",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NitTercero = c.String(nullable: false, maxLength: 20),
                        IngresosMensuales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GastosMensuales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PasivosTotales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActivosTotales = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ter.Terceros", t => t.NitTercero, cascadeDelete: true)
                .Index(t => t.NitTercero);
            
            AlterColumn("ter.InfoTerceroAdicional", "Ocupacion", c => c.String(nullable: false, maxLength: 50));
            
        }
        
        public override void Down()
        {
            DropForeignKey("ter.InfoTerceroFinanciera", "NitTercero", "ter.Terceros");
            DropIndex("ter.InfoTerceroFinanciera", new[] { "NitTercero" });
           AlterColumn("ter.InfoTerceroAdicional", "Ocupacion", c => c.String(maxLength: 50));
            DropTable("ter.InfoTerceroFinanciera");
            RenameIndex(table: "ter.InfoTerceroAdicional", name: "IX_IdEstrato", newName: "IX_IdEstarto");
            RenameColumn(table: "ter.InfoTerceroAdicional", name: "IdEstrato", newName: "IdEstarto");
        }
    }
}
