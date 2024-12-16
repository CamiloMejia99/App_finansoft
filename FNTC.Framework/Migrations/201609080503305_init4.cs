namespace FNTC.Framework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("par.Parameters", "Categoria", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("par.Parameters", "Categoria");
        }
    }
}
