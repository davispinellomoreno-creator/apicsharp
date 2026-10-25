using Microsoft.EntityFrameworkCore;
using apicsharp.Models;
namespace apicsharp.Data;
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) :
base(options) { }
public DbSet<Usuario> Usuarios => Set<Usuario>();
public DbSet<Categoria> Categorias => Set<Categoria>();
public DbSet<Transacao> Transacoes => Set<Transacao>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Usuario>(e =>
    {
        e.Property(u => u.Nome).HasMaxLength(100);
        e.Property(u => u.Email).HasMaxLength(200);
        e.HasIndex(u => u.Email).IsUnique();
    });

    modelBuilder.Entity<Categoria>(e =>
    {
        e.Property(c => c.Nome).HasMaxLength(100);
        // Salva o enum como texto ("Receita"/"Despesa") em vez de número
        e.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);
    });

    modelBuilder.Entity<Transacao>(e =>
    {
        e.Property(t => t.Descricao).HasMaxLength(200);
        e.Property(t => t.Valor).HasPrecision(18, 2);

        // Impede apagar uma categoria que ainda tem transações
        e.HasOne(t => t.Categoria)
            .WithMany()
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(t => t.Usuario)
            .WithMany()
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    });
}
}
