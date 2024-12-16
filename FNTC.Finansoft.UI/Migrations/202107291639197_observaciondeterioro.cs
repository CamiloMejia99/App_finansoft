namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class observaciondeterioro : DbMigration
    {
        public override void Up()
        {
            AddColumn("DCar.DeterioroCartera", "observacion", c => c.String(maxLength: 250));
            
        }
        
        public override void Down()
        {
           
            DropColumn("DCar.DeterioroCartera", "observacion");
        }
    }
}
