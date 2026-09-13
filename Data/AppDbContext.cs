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
}
