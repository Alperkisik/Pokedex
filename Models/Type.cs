using System.ComponentModel.DataAnnotations;

namespace Pokedex.Models
{
    public class Type
    {
        public int Id { get; set; }
        [MaxLength(50),Required]
        public string Content { get; set; }

        public ICollection<Pokemon>? Pokemons { get; set; }
    }
}
