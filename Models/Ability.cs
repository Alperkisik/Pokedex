using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.Models
{
    public class Ability
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Effect { get; set; }
        //active or hidden
        [DefaultValue("Active")]
        public string Type { get; set; }

        public ICollection<Pokemon>? Pokemons { get; set; }
    }
}
