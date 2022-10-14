using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Agrego los diferentes modelos que necesito (si no se agregan los modelos la migracion va a ser vacia).
        public DbSet<AppUsuario> AppUsuario { get; set; }   

    }

}
