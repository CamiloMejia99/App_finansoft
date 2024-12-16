namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuentasdeteriorotipocom : DbMigration
    {
        public override void Up()
        {
            AddColumn("DCar.CuentaDeterioroCartera", "TComprobante", c => c.String(maxLength: 255));
            CreateIndex("DCar.CuentaDeterioroCartera", "TComprobante");
            AddForeignKey("DCar.CuentaDeterioroCartera", "TComprobante", "acc.TiposComprobantes", "CODIGO");
        }
        
        public override void Down()
        {
            DropForeignKey("DCar.CuentaDeterioroCartera", "TComprobante", "acc.TiposComprobantes");
            DropIndex("DCar.CuentaDeterioroCartera", new[] { "TComprobante" });
            DropColumn("DCar.CuentaDeterioroCartera", "TComprobante");
        }
    }
}
