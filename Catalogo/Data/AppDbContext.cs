using Catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Categorias> Categorias { get; set; }
    public DbSet<Produtos> Produtos { get; set; }
}
