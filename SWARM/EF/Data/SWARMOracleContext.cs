using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SWARM.EF.Models;

#nullable disable

namespace SWARM.EF
{
    public partial class SWARMOracleContext : DbContext
    {
        public SWARMOracleContext()
        {
        }

        public SWARMOracleContext(DbContextOptions<SWARMOracleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PhoneType> PhoneTypes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentPhone> StudentPhones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("C##STU")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<PhoneType>(entity =>
            {
                entity.Property(e => e.PhoneType1).IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentName).IsUnicode(false);
            });

            modelBuilder.Entity<StudentPhone>(entity =>
            {
                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.StudentPhonePrimary)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.PhoneType)
                    .WithMany(p => p.StudentPhones)
                    .HasForeignKey(d => d.PhoneTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STUDENT_PHONE_FK2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentPhones)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STUDENT_PHONE_FK1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
