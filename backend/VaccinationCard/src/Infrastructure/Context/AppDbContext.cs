using Domain.Entities;
using Domain.Interfaces;
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

            e.HasIndex(p => p.NameSearchableColumn); // Sim, por mais que eu vou fazer buscas LIKE, o índice padrão B-TREE funciona
                                                 // Ele só não funcionaria se a busca fosse '%termo%',
                                                 // mas como vou usar 'termo%' ele funciona bem.

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
             .WithMany(v => v.Vaccinations)
             .HasForeignKey(v => v.VaccineId);

            e.OwnsOne(v => v.Dose, vd =>
            {
                vd.Property(x => x.Type).HasColumnName("DoseType");
                vd.Property(x => x.DoseNumber).HasColumnName("DoseNumber");
            });

            e.HasOne(v => v.Person)
             .WithMany(p => p.Vaccinations)
             .HasForeignKey(v => v.PersonId);

            e.AddSoftDelete();
        });
    }
}

internal static class EntityExtensions
{
    public static void AddSoftDelete<T>(this EntityTypeBuilder<T> builder) where T : class, ISoftDeletable
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}