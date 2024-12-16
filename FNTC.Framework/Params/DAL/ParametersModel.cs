namespace FNTC.Framework.Params
{
    using FNTC.Framework.Paras.DAL;
    using System.Data.Entity;

    public partial class ParametersModel : DbContext
    {
        public ParametersModel()
            : base("name=GeoModel")
        {
        }

        public virtual DbSet<Parameter> Parametros { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("par");
            base.OnModelCreating(modelBuilder);
        }
    }
}