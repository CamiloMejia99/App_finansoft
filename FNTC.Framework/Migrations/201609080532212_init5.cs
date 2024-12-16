namespace FNTC.Framework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("par.Parameters", "Descripcion", c => c.String());
        }

        public override void Down()
        {
            DropColumn("par.Parameters", "Descripcion");
        }
    }
}
