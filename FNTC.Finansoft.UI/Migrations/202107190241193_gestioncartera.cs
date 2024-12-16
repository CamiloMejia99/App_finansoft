namespace FNTC.Finansoft.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gestioncartera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Gcar.CompromisoCartera", "ObservacionCompromiso", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("Gcar.CompromisoCartera", "TipoCompromiso", c => c.String(maxLength: 20));
            AlterColumn("Gcar.GCgestion", "RespuestaGestion", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("Gcar.GCgestion", "RespuestaGestion", c => c.String(nullable: false));
            AlterColumn("Gcar.CompromisoCartera", "TipoCompromiso", c => c.String());
            AlterColumn("Gcar.CompromisoCartera", "ObservacionCompromiso", c => c.String(nullable: false));
        }
    }
}
