using Catalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Data;

public class AppDbContext :  IdentityDbContext<AplicationUsers>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Categorias> Categorias { get; set; }
    public DbSet<Produtos> Produtos { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
