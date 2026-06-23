using Microsoft.EntityFrameworkCore;
using FinancasAPI.Models;

namespace FinancasAPI.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<Transacao> Transacoes { get; set; }

   
    public DbSet<Usuario> Usuarios { get; set; }
}
