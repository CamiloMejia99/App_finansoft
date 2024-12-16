namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fechaadicional : DbMigration
    {
        public override void Up()
        {
            AddColumn("ter.InfoTerceroAdicional", "Fechalaboral", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ter.InfoTerceroAdicional", "Fechalaboral");
        }
    }
}
