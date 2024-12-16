namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deterioroplancuentas : DbMigration
    {
        public override void Up()
        {
            AddColumn("DCar.CuentaDeterioroCartera", "IdPlanCuenta", c => c.String(maxLength: 255));
            AlterColumn("DCar.DeterioroCartera", "Metodo", c => c.String(maxLength: 20));
            AlterColumn("DCar.DeterioroCartera", "ValorSuma", c => c.String(maxLength: 50));
            CreateIndex("DCar.CuentaDeterioroCartera", "IdPlanCuenta");
            AddForeignKey("DCar.CuentaDeterioroCartera", "IdPlanCuenta", "acc.PlanCuentas", "CODIGO");
            DropColumn("DCar.CuentaDeterioroCartera", "CuentaDeterioro");
        }
        
        public override void Down()
        {
            AddColumn("DCar.CuentaDeterioroCartera", "CuentaDeterioro", c => c.String(nullable: false, maxLength: 30));
            DropForeignKey("DCar.CuentaDeterioroCartera", "IdPlanCuenta", "acc.PlanCuentas");
            DropIndex("DCar.CuentaDeterioroCartera", new[] { "IdPlanCuenta" });
            AlterColumn("DCar.DeterioroCartera", "ValorSuma", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("DCar.DeterioroCartera", "Metodo", c => c.String(nullable: false, maxLength: 10));
            DropColumn("DCar.CuentaDeterioroCartera", "IdPlanCuenta");
        }
    }
}
