using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<Vaccination> Vaccinations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(e =>
        {
            e.ToTable("Person");

            e.HasKey(p => p.Id);
            e.OwnsOne(p => p.Name, n =>
            {
                n.Property(nn => nn.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(100);
                n.Property(nn => nn.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(100);
            });

            e.Property(p => p.CPF); // Ia colocar uma validação de tamanho máximo, mas já vi nptícias de que o formato do cpf pode mudar

            e.HasMany(p => p.Vaccinations)
             .WithOne(v => v.Person)
             .HasForeignKey(v => v.PersonId);

            e.AddSoftDelete();
        });

        modelBuilder.Entity<Vaccine>(e =>
        {
            e.ToTable("Vaccine");

            e.HasKey(v => v.Id);
            e.AddSoftDelete();
        });

        modelBuilder.Entity<Vaccination>(e =>
        {
            e.ToTable("Vaccination");
            e.HasKey(v => v.Id);
            e.HasOne(v => v.Vaccine)
             .WithMany()
             .HasForeignKey(v => v.VaccineId);

            e.HasOne(v => v.Person)
             .WithMany(p => p.Vaccinations)
             .HasForeignKey(v => v.PersonId);

            e.AddSoftDelete();
        });
    }
}

internal static class EntityExtensions
{
    public static void AddSoftDelete<T>(this EntityTypeBuilder<T> builder) where T : AuditableEntity
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}