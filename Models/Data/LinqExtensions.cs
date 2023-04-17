namespace Pokedex.Models.Data
{
    public static class LinqExtensions
    {
        public static List<Pokemon> To_List_IncludeRelationships(this IEnumerable<Pokemon> pokemons)
        {
            List<Pokemon> list = new List<Pokemon>();

            foreach (var pokemon in pokemons)
            {
                list.Add(pokemon);
            }

            return list;
        }
    }
}
