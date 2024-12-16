namespace FNTC.Framework.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "par.Parameters",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Codigo = c.String(nullable: false),
                    Nombre = c.String(nullable: false),
                    Valor = c.String(nullable: false),
                    TipoValor = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("par.Parameters");
        }
    }
}
