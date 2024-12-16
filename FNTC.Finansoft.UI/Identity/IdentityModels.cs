using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace IdentitySample.Models
{
    public class ApplicationUser : IdentityUser
    {

        //Agregado Jmesoft
        public ApplicationUser()
            : base()
        {

            PreviousUserPasswords = new List<PreviousPassword>();

        }

        public virtual IList<PreviousPassword> PreviousUserPasswords { get; set; }
        //Fin Agregado Jmesoft

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Nombres { get; set; }
        public string Cedula { get; set; }
        

        // Use a sensible display name for views:
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime? LastActivityDate { get; set; }

        public DateTime? Fecha_Registro { get; set; }
        // Concatenate the address info for display in tables and such:
        public string DisplayAddress
        {
            get
            {
                string dspAddress = string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;
                string dspCity = string.IsNullOrWhiteSpace(this.City) ? "" : this.City;
                string dspState = string.IsNullOrWhiteSpace(this.State) ? "" : this.State;
                string dspPhoneNumber = string.IsNullOrWhiteSpace(this.PhoneNumber) ? "" : this.PhoneNumber;
                return string.Format("{0} {1} {2} {3}", dspAddress, dspCity, dspState, PhoneNumber);
            }
        }

        
    }

    //Agregado Jmesoft
    public class PreviousPassword
    {
        public PreviousPassword()
        {
            CreateDate = DateTimeOffset.Now;
        }

        [Key, Column(Order = 0)]
        public string PasswordHash { get; set; }

        public DateTimeOffset CreateDate { get; set; }


        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        //Fin Agregado Jmesoft
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AccContext", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
       
    }


    

    


}