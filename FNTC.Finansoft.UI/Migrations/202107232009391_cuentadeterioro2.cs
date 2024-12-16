namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuentadeterioro2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "DCar.CuentaDeterioroCartera", name: "IdPlanCuenta", newName: "IdPlanCuentaDeterioro");
            RenameIndex(table: "DCar.CuentaDeterioroCartera", name: "IX_IdPlanCuenta", newName: "IX_IdPlanCuentaDeterioro");
            AddColumn("DCar.CuentaDeterioroCartera", "IdPlanCuentaGastosProvision", c => c.String(maxLength: 255));
            AddColumn("DCar.CuentaDeterioroCartera", "NombreSeleccion", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("DCar.CuentaDeterioroCartera", "IdPlanCuentaGastosProvision");
            AddForeignKey("DCar.CuentaDeterioroCartera", "IdPlanCuentaGastosProvision", "acc.PlanCuentas", "CODIGO");
            DropColumn("DCar.CuentaDeterioroCartera", "Nombre");
        }
        
        public override void Down()
        {
            AddColumn("DCar.CuentaDeterioroCartera", "Nombre", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("DCar.CuentaDeterioroCartera", "IdPlanCuentaGastosProvision", "acc.PlanCuentas");
            DropIndex("DCar.CuentaDeterioroCartera", new[] { "IdPlanCuentaGastosProvision" });
            DropColumn("DCar.CuentaDeterioroCartera", "NombreSeleccion");
            DropColumn("DCar.CuentaDeterioroCartera", "IdPlanCuentaGastosProvision");
            RenameIndex(table: "DCar.CuentaDeterioroCartera", name: "IX_IdPlanCuentaDeterioro", newName: "IX_IdPlanCuenta");
            RenameColumn(table: "DCar.CuentaDeterioroCartera", name: "IdPlanCuentaDeterioro", newName: "IdPlanCuenta");
        }
    }
}
