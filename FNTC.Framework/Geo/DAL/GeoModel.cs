namespace FNTC.Framework.Geo
{
    using System.Data.Entity;

    public partial class GeoModel : DbContext
    {
        public GeoModel()
            : base("name=GeoModel")
        {
        }

        public virtual DbSet<DivisionPolitica> divisionpoliticas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("geo");
            base.OnModelCreating(modelBuilder);
        }
    }
}
