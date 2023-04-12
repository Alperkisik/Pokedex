namespace Pokedex.Models
{
    public class Weakness
    {
        public int Id { get; set; }

        public Type Type { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
