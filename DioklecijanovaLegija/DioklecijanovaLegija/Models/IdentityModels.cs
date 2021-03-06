﻿using DioklecijanovaLegija.EntityTypeConfigurations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DioklecijanovaLegija.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<SoldierType> SoldierTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Calendar> Calendars { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new MemberConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());

            modelBuilder.Entity<Member>()
               .HasMany<Event>(m => m.Events)
               .WithMany(e => e.Members)
               .Map(me =>
               {
                   me.MapLeftKey("MemberRefId");
                   me.MapRightKey("EventRefId");
                   me.ToTable("MemberEvent");
               });

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
