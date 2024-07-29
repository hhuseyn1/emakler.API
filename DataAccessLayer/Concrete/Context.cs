using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }
    public virtual DbSet<BuildingPost> BuildingPosts { get; set; }
    public virtual DbSet<BuildingType> BuildingTypes { get; set; }
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<Metro> Metros { get; set; }
    public virtual DbSet<OperationType> OperationTypes { get; set; }
    public virtual DbSet<OwnerType> OwnerTypes { get; set; }
    public virtual DbSet<Property> Properties { get; set; }
    public virtual DbSet<PropertyType> PropertyTypes { get; set; }
    public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<RegionUnit01> RegionUnit01s { get; set; }
    public virtual DbSet<RepairRate> RepairRates { get; set; }
    public virtual DbSet<RoomCount> RoomCounts { get; set; }
    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Building>(entity =>
        {
            entity.Property(e => e.Area)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)");

            entity.HasKey(b => b.Id);
        });


        modelBuilder.Entity<BuildingPost>()
                .HasKey(bp => bp.Id);

        modelBuilder.Entity<BuildingType>()
                .HasKey(bt => bt.Id);

        modelBuilder.Entity<Document>()
                .HasKey(d => d.Id);

        modelBuilder.Entity<Metro>()
                .HasKey(m => m.Id);

        modelBuilder.Entity<OperationType>()
                .HasKey(ot => ot.Id);

        modelBuilder.Entity<OwnerType>()
                .HasKey(ot => ot.Id);

        modelBuilder.Entity<Property>()
                .HasKey(p => p.Id);

        modelBuilder.Entity<PropertyType>()
                .HasKey(pt => pt.Id);

        modelBuilder.Entity<Region>()
                .HasKey(r => r.Id);

        modelBuilder.Entity<RegionUnit01>()
                .HasKey(r => r.Id);

        modelBuilder.Entity<RepairRate>()
                .HasKey(rr => rr.Id);

        modelBuilder.Entity<RoomCount>()
                .HasKey(rc => rc.Id);

        modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
    }
}
