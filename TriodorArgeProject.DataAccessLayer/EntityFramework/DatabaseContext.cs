using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriodorArgeProject.Entities;
using TriodorArgeProject.Entities.EntityClasses;

namespace TriodorArgeProject.DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=TriodorDatabase")
        {
        }

        public virtual DbSet<ArgeCost> ArgeCost { get; set; }
        public virtual DbSet<EquipmentCosts> EquipmentCosts { get; set; }
        public virtual DbSet<FundingSchemes> FundingSchemes { get; set; }
        public virtual DbSet<MaterialCosts> MaterialCosts { get; set; }
        public virtual DbSet<PersonnelAbility> PersonnelAbility { get; set; }
        public virtual DbSet<PersonnelCertificates> PersonnelCertificates { get; set; }
        public virtual DbSet<PersonnelCosts> PersonnelCosts { get; set; }
        public virtual DbSet<PersonnelEducations> PersonnelEducations { get; set; }
        public virtual DbSet<PersonnelProjectsTable> PersonnelProjectsTable { get; set; }
        public virtual DbSet<Personnels> Personnels { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SuppProjectCosts> SuppProjectCosts { get; set; }
        public virtual DbSet<SuppProjectPersonnels> SuppProjectPersonnels { get; set; }
        public virtual DbSet<SuppProjects> SuppProjects { get; set; }
        public virtual DbSet<SuppProjectsStatus> SuppProjectsStatus { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TravelCosts> TravelCosts { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WorkExperiences> WorkExperiences { get; set; }
        public virtual DbSet<ServiceCosts> ServiceCosts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArgeCost>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ArgeCost>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ArgeCost>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);

            modelBuilder.Entity<EquipmentCosts>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<EquipmentCosts>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<EquipmentCosts>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);

            modelBuilder.Entity<MaterialCosts>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<MaterialCosts>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<MaterialCosts>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);

            modelBuilder.Entity<PersonnelCosts>()
                .Property(e => e.ActualManMonth)
                .HasPrecision(18, 1);

            modelBuilder.Entity<PersonnelCosts>()
                .Property(e => e.PlannedManMonth)
                .HasPrecision(18, 1);

            modelBuilder.Entity<PersonnelCosts>()
                .Property(e => e.AcceptedManMonth)
                .HasPrecision(18, 1);

            modelBuilder.Entity<PersonnelEducations>()
                .Property(e => e.StartYear)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PersonnelEducations>()
                .Property(e => e.EndYear)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PersonnelProjectsTable>()
                .Property(e => e.WorkRate)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PersonnelProjectsTable>()
                .Property(e => e.Year)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PersonnelProjectsTable>()
                .Property(e => e.Month)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.PersonnelAbility)
                .WithRequired(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.PersonnelCertificates)
                .WithRequired(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.PersonnelCosts)
                .WithOptional(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.PersonnelEducations)
                .WithRequired(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.PersonnelProjectsTable)
                .WithOptional(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.SuppProjectPersonnels)
                .WithRequired(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.TravelCosts)
                .WithOptional(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID);

            modelBuilder.Entity<Personnels>()
                .HasMany(e => e.WorkExperiences)
                .WithRequired(e => e.Personnels)
                .HasForeignKey(e => e.PersonnelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Positions>()
                .HasMany(e => e.Personnels)
                .WithRequired(e => e.Positions)
                .HasForeignKey(e => e.PositionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.Cost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<Projects>()
                .HasMany(e => e.PersonnelProjectsTable)
                .WithOptional(e => e.Projects)
                .HasForeignKey(e => e.ProjectID);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Roles)
                .HasForeignKey(e => e.RoleID);

            modelBuilder.Entity<SuppProjectCosts>()
                .Property(e => e.Year)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SuppProjectCosts>()
                .Property(e => e.Period)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SuppProjectCosts>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<SuppProjectCosts>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<SuppProjectCosts>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);



            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.ArgeCost)
                .WithOptional(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID)
                .WillCascadeOnDelete();
                

            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.EquipmentCosts)
                .WithOptional(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.MaterialCosts)
                .WithRequired(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.PersonnelCosts)
                .WithOptional(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.ServiceCosts)
                .WithOptional(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID);

            modelBuilder.Entity<SuppProjectCosts>()
                .HasMany(e => e.TravelCosts)
                .WithOptional(e => e.SuppProjectCosts)
                .HasForeignKey(e => e.CostID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SuppProjects>()
                .Property(e => e.Budget)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SuppProjects>()
                .HasMany(e => e.SuppProjectCosts)
                .WithOptional(e => e.SuppProjects)
                .HasForeignKey(e => e.ProjectID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<SuppProjects>()
                .HasMany(e => e.SuppProjectPersonnels)
                .WithRequired(e => e.SuppProjects)
                .HasForeignKey(e => e.SuppProjectD);

            modelBuilder.Entity<SuppProjectsStatus>()
                .HasMany(e => e.SuppProjects)
                .WithOptional(e => e.SuppProjectsStatus)
                .HasForeignKey(e => e.StatusID);

            modelBuilder.Entity<TravelCosts>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<TravelCosts>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<TravelCosts>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Personnels)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<WorkExperiences>()
                .Property(e => e.StartYear)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<WorkExperiences>()
                .Property(e => e.EndYear)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCosts>()
                .Property(e => e.Actual)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ServiceCosts>()
                .Property(e => e.Planned)
                .HasPrecision(18, 1);

            modelBuilder.Entity<ServiceCosts>()
                .Property(e => e.Accepted)
                .HasPrecision(18, 1);
        }
    }
}
