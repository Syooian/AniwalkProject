using System;
using System.Collections.Generic;
using AniwalkServerDBFirstTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServerDBFirstTest.Data;

public partial class AniwalkContext : DbContext
{
    public AniwalkContext(DbContextOptions<AniwalkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animes> Animes { get; set; }

    public virtual DbSet<Comments> Comments { get; set; }

    public virtual DbSet<Countries> Countries { get; set; }

    public virtual DbSet<Login> Login { get; set; }

    public virtual DbSet<MemberRoles> MemberRoles { get; set; }

    public virtual DbSet<Members> Members { get; set; }

    public virtual DbSet<Visits> Visits { get; set; }

    public virtual DbSet<VisitsPhotos> VisitsPhotos { get; set; }

    public virtual DbSet<VisitsTags> VisitsTags { get; set; }

    public virtual DbSet<Visits_VisitsPhotos> Visits_VisitsPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animes>(entity =>
        {
            entity.Property(e => e.AnimeID).IsFixedLength();
            entity.Property(e => e.HeaderPhoto).IsFixedLength();
        });

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.Property(e => e.CommentText).HasDefaultValue("");
            entity.Property(e => e.MemberID).IsFixedLength();
        });

        modelBuilder.Entity<Countries>(entity =>
        {
            entity.Property(e => e.CountryCode).IsFixedLength();
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();
        });

        modelBuilder.Entity<Members>(entity =>
        {
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.CountryCode).IsFixedLength();
        });

        modelBuilder.Entity<Visits>(entity =>
        {
            entity.Property(e => e.AnimeID).IsFixedLength();
            entity.Property(e => e.CountryCode).IsFixedLength();
            entity.Property(e => e.MemberID).IsFixedLength();

            entity.HasOne(d => d.Anime).WithMany(p => p.Visits).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Visits).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Member).WithMany(p => p.Visits).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<VisitsPhotos>(entity =>
        {
            entity.Property(e => e.PhotoID).IsFixedLength();
            entity.Property(e => e.MemberID).IsFixedLength();
        });

        modelBuilder.Entity<VisitsTags>(entity =>
        {
            entity.HasMany(d => d.VisitSN).WithMany(p => p.TagSN)
                .UsingEntity<Dictionary<string, object>>(
                    "VisitsDetails",
                    r => r.HasOne<Visits>().WithMany().HasForeignKey("VisitSN"),
                    l => l.HasOne<VisitsTags>().WithMany().HasForeignKey("TagSN"),
                    j =>
                    {
                        j.HasKey("TagSN", "VisitSN");
                        j.HasIndex(new[] { "VisitSN" }, "IX_VisitsDetails_VisitSN");
                    });
        });

        modelBuilder.Entity<Visits_VisitsPhotos>(entity =>
        {
            entity.ToView("Visits_VisitsPhotos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
