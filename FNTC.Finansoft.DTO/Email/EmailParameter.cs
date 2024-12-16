namespace FNTC.Finansoft.Accounting.DTO.Email
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmailParameter")]
    public partial class EmailParameter
    {
        [Key]
        [Column(Order = 0)]
        public int Parametro_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string emailFrom { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string emailPassword { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string SmtpClient { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Port { get; set; }


    }
}
