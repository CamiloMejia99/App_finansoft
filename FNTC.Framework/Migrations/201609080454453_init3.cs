namespace FNTC.Framework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("par.Parameters", "NombreParametro", c => c.String(nullable: false));
            DropColumn("par.Parameters", "Nombre");
        }

        public override void Down()
        {
            AddColumn("par.Parameters", "Nombre", c => c.String(nullable: false));
            DropColumn("par.Parameters", "NombreParametro");
        }
    }
}
