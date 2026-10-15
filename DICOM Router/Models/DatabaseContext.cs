using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DICOM_Router.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<EfmigrationsLock> EfmigrationsLocks { get; set; }

    public virtual DbSet<IncomingStudy> IncomingStudies { get; set; }

    public virtual DbSet<OutgingStudy> OutgingStudies { get; set; }

    public virtual DbSet<RoutingRule> RoutingRules { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\Jon\\source\\repos\\DICOM Router\\DICOM Router\\DB\\Database.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CalledAe).HasColumnName("Called_AE");
            entity.Property(e => e.CallingAe).HasColumnName("Calling_AE");
            entity.Property(e => e.DeviceName).HasColumnName("Device_Name");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IpAddress).HasColumnName("IP_Address");
        });

        modelBuilder.Entity<EfmigrationsLock>(entity =>
        {
            entity.ToTable("__EFMigrationsLock");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<IncomingStudy>(entity =>
        {
            entity.ToTable("Incoming Studies");

            entity.HasIndex(e => e.Id, "IX_Incoming Studies_ID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateRec).HasColumnName("Date_Rec");
            entity.Property(e => e.ImageCount).HasColumnName("Image_Count");
            entity.Property(e => e.ModalityType).HasColumnName("Modality_Type");
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");
            entity.Property(e => e.PatientName).HasColumnName("Patient_Name");
            entity.Property(e => e.Suid).HasColumnName("SUID");
        });

        modelBuilder.Entity<OutgingStudy>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Outging_Studies");

            entity.Property(e => e.DateSent).HasColumnName("Date_Sent");
            entity.Property(e => e.DeviceName).HasColumnName("Device_Name");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageCount).HasColumnName("Image_Count");
            entity.Property(e => e.ModalityType).HasColumnName("Modality_Type");
            entity.Property(e => e.PatientId).HasColumnName("Patient_ID");
            entity.Property(e => e.PatientName).HasColumnName("Patient_Name");
            entity.Property(e => e.RuleName).HasColumnName("Rule_Name");
            entity.Property(e => e.Suid).HasColumnName("SUID");
        });

        modelBuilder.Entity<RoutingRule>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Routing_Rules");

            entity.Property(e => e.DestTag).HasColumnName("Dest_Tag");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MatchValue).HasColumnName("Match_Value");
            entity.Property(e => e.ReplaceValue).HasColumnName("Replace_Value");
            entity.Property(e => e.SourceTag).HasColumnName("Source_Tag");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Server");

            entity.Property(e => e.AeTitle).HasColumnName("AE_Title");
            entity.Property(e => e.LoggingMode).HasColumnName("Logging_Mode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
