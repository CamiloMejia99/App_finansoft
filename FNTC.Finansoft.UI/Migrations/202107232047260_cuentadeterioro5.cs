namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuentadeterioro5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DCar.CuentaDeterioroCartera", "NombreSeleccion", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("DCar.CuentaDeterioroCartera", "NombreSeleccion", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
