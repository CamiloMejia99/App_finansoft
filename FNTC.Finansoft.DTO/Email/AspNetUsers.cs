namespace FNTC.Finansoft.Accounting.DTO.Email
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AspNetUsers
    {
        [Key]
        public string Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Nombres { get; set; }
        // public string Cedula { get; s
        // et; }//

        public DateTime Fecha_Registro { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

    }
}
