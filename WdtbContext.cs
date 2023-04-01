using System;
using System.Collections.Generic;
using MusBase.Models;
using Microsoft.EntityFrameworkCore;

namespace MusBase;

public partial class WdtbContext : DbContext
{
    public WdtbContext()
    {
    }

    public WdtbContext(DbContextOptions<WdtbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<RecordsArtist> RecordsArtists { get; set; }

    public virtual DbSet<RecordsGenre> RecordsGenres { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= WIN-EJ3KQB83H47\\SQLEXPRESS;\nDatabase=WDTB; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.Property(e => e.DateBase).HasColumnType("date");
            entity.Property(e => e.DateEnd).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Artists_Countries");

            entity.HasOne(d => d.Label).WithMany(p => p.Artists)
                .HasForeignKey(d => d.LabelId)
                .HasConstraintName("FK_Artists_Labels");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Quality).HasMaxLength(10);
        });

        modelBuilder.Entity<RecordsArtist>(entity =>
        {
            entity.ToTable("Records_Artists");

            entity.HasOne(d => d.Artist).WithMany(p => p.RecordsArtists)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Records_Artists_Artists");

            entity.HasOne(d => d.Record).WithMany(p => p.RecordsArtists)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Records_Artists_Records");
        });

        modelBuilder.Entity<RecordsGenre>(entity =>
        {
            entity.ToTable("Records_Genres");

            entity.HasOne(d => d.Genre).WithMany(p => p.RecordsGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Records_Genres_Genres");

            entity.HasOne(d => d.Record).WithMany(p => p.RecordsGenres)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Records_Genres_Records");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasOne(d => d.Record).WithMany(p => p.Sales)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Records");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
