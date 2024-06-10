using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _04_SE1814_EmployeeManagement.sln.BusinessObjects;

public partial class EmployeeManagementContext : DbContext
{
    public EmployeeManagementContext()
    {
    }

    public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D160BF9A088413");

            entity.Property(e => e.CountryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CountryID");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegionId).HasColumnName("RegionID");

            entity.HasOne(d => d.Region).WithMany(p => p.Countries)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__Countries__Regio__2E1BDC42");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD79AFDBB7");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocationId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("LocationID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.HasOne(d => d.Location).WithMany(p => p.Departments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Departmen__Locat__2F10007B");

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Departmen__Manag__300424B4");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1A77D495E");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.CommissionPct).HasColumnName("Commission_pct");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.JobId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("JobID");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Employees__Depar__30F848ED");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK__Employees__JobID__31EC6D26");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employees__Manag__32E0915F");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Jobs__056690E21241D41A");

            entity.Property(e => e.JobId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("JobID");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaxSalary).HasColumnName("max_salary");
            entity.Property(e => e.MinSalary).HasColumnName("min_salary");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA477AE2078A6");

            entity.Property(e => e.LocationId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("LocationID");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CountryId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CountryID");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateProvince)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__Locations__Count__33D4B598");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK__Regions__ACD84443FE898DC3");

            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnName("RegionID");
            entity.Property(e => e.RegionName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
