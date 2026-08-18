using DL_SmartAppraisal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DL_SmartAppraisal.Data
{
    public class SmartAppraisalDbContext : DbContext
    {
        public SmartAppraisalDbContext(
            DbContextOptions<SmartAppraisalDbContext> options)
            : base(options)
        {
        }

        // ==============================
        // User Management Tables
        // ==============================

        public DbSet<UserDetail> UserDetails { get; set; } = null!;

        public DbSet<Role> Roles { get; set; } = null!;


        // ==============================
        // Case Study Tables
        // ==============================

        public DbSet<CaseStudy> CaseStudies { get; set; } = null!;

        public DbSet<CaseStudySolution> CaseStudySolutions { get; set; } = null!;

        public DbSet<CaseStudyCompetency> CaseStudyCompetencies { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ==========================================
            // CaseStudy Primary Key
            // ==========================================

            modelBuilder.Entity<CaseStudy>()
                .HasKey(x => x.CaseStudyId);


            // ==========================================
            // CaseStudySolution Primary Key
            // ==========================================

            modelBuilder.Entity<CaseStudySolution>()
                .HasKey(x => x.CaseStudySolutionId);


            // ==========================================
            // CaseStudyCompetency Primary Key
            // ==========================================

            modelBuilder.Entity<CaseStudyCompetency>()
                .HasKey(x => x.CaseStudyCompetencyId);


            // ==========================================
            // CaseStudy -> Solutions
            // ==========================================

            modelBuilder.Entity<CaseStudy>()
                .HasMany(x => x.Solutions)
                .WithOne(x => x.CaseStudy)
                .HasForeignKey(x => x.CaseStudyId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==========================================
            // Solution -> Competencies
            // ==========================================

            modelBuilder.Entity<CaseStudySolution>()
                .HasMany(x => x.Competencies)
                .WithOne(x => x.CaseStudySolution)
                .HasForeignKey(x => x.CaseStudySolutionId)
                .OnDelete(DeleteBehavior.Cascade);


            // ==========================================
            // UserDetail -> CaseStudy
            //
            // UserDetail.Id       INT
            //        ↓
            // CaseStudy.CreatedBy  INT
            // ==========================================

            modelBuilder.Entity<CaseStudy>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}