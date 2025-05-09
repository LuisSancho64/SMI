using Microsoft.EntityFrameworkCore;
using SMI.Client.Pages;
using SMI.Shared.Models;

namespace SMI.Server.Data
{
    public class SGISDbContext : DbContext
    {
        public SGISDbContext(DbContextOptions<SGISDbContext> options) : base(options)
        {
        }

        public DbSet<User> Usuario { get; set; }
        // Aquí agregarías otros DbSet para las tablas de tu base de datos
    }
}