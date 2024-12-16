namespace FNTC.Framework.Geo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("geo.divisionpolitica")]
    public partial class DivisionPolitica
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(30)]
        public string DeptoId { get; set; }

        [StringLength(30)]
        public string MunicipioId { get; set; }

        [StringLength(30)]
        public string CentroPobladoId { get; set; }

        [StringLength(30)]
        public string DeptoNombre { get; set; }

        [StringLength(30)]
        public string PaisId { get; set; }

        [StringLength(255)]
        public string PaisNombre { get; set; }

        [StringLength(255)]
        public string MunicipioNombre { get; set; }

        [StringLength(255)]
        public string CentroPobladoNombre { get; set; }

        [StringLength(255)]
        public string CentroPobladoTipo { get; set; }

        [StringLength(255)]
        public string Longitud { get; set; }

        [StringLength(255)]
        public string Latitud { get; set; }

        [StringLength(255)]
        public string Distrito { get; set; }

        [StringLength(255)]
        public string MunicipioTipo { get; set; }

        [StringLength(255)]
        public string AreaMetropolitana { get; set; }
    }
}
