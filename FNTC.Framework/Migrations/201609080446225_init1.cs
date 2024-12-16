namespace FNTC.Framework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("par.Parameters", "Valor", c => c.String());
            AlterColumn("par.Parameters", "TipoValor", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("par.Parameters", "TipoValor", c => c.String(nullable: false));
            AlterColumn("par.Parameters", "Valor", c => c.String(nullable: false));
        }
    }
}
