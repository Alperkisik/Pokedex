using Microsoft.EntityFrameworkCore;

namespace Pokedex.Models.Data
{
    public class PokedexContext : DbContext
    {
        public PokedexContext(DbContextOptions<PokedexContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Type> Types { get; set; }
    }
}
