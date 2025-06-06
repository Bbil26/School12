using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Главное_окно;

public partial class School12Context : DbContext
{
    public School12Context()
    {
    }

    public School12Context(DbContextOptions<School12Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<UserDatum> UserData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=School12;Username=postgres;Password=nahura54");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.IdDisease).HasName("disease_pkey");

            entity.ToTable("disease", "school12");

            entity.Property(e => e.IdDisease)
                .ValueGeneratedNever()
                .HasColumnName("id_disease");
            entity.Property(e => e.DateDisease).HasColumnName("date_disease");
            entity.Property(e => e.DescriptionDisease).HasColumnName("description_disease");
            entity.Property(e => e.DiseaseIdStudent).HasColumnName("disease_id_student");
            entity.Property(e => e.HardDisease)
                .HasDefaultValue(false)
                .HasColumnName("hard_disease");

            entity.HasOne(d => d.DiseaseIdStudentNavigation).WithMany(p => p.Diseases)
                .HasForeignKey(d => d.DiseaseIdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("disease-student");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("role_pkey");

            entity.ToTable("role", "school12");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.NameRole).HasColumnName("name_role");
        });

        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.HasKey(e => e.IdClass).HasName("school_class_pkey");

            entity.ToTable("school_class", "school12");

            entity.Property(e => e.IdClass).HasColumnName("id_class");
            entity.Property(e => e.NameClass).HasColumnName("name_class");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.IdStudent).HasName("student_pkey");

            entity.ToTable("student", "school12");

            entity.Property(e => e.IdStudent).HasColumnName("id_student").UseIdentityColumn();
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.SecondName).HasColumnName("second_name");
            entity.Property(e => e.StudentIdClass).HasColumnName("student_id_class");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.YearOfBirth).HasColumnName("year_of_birth");

            entity.HasOne(d => d.StudentIdClassNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentIdClass)
                .HasConstraintName("student-school_class");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.IdTeacher).HasName("teacher_pkey");

            entity.ToTable("teacher", "school12");

            entity.Property(e => e.IdTeacher).HasColumnName("id_teacher");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.SecondName).HasColumnName("second_name");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.TeacherIdClass).HasColumnName("teacher_id_class");
            entity.Property(e => e.TeacherIdUser).HasColumnName("teacher_id_user");
            entity.Property(e => e.YearOfBirth).HasColumnName("year_of_birth");

            entity.HasOne(d => d.TeacherIdClassNavigation).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.TeacherIdClass)
                .HasConstraintName("teacher-school_class");

            entity.HasOne(d => d.TeacherIdUserNavigation).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.TeacherIdUser)
                .HasConstraintName("teacher-user_data");
        });

        modelBuilder.Entity<UserDatum>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_data_pkey");

            entity.ToTable("user_data", "school12");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.UserIdRole).HasColumnName("user_id_role");

            entity.HasOne(d => d.UserIdRoleNavigation).WithMany(p => p.UserData)
                .HasForeignKey(d => d.UserIdRole)
                .HasConstraintName("user_data-role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
