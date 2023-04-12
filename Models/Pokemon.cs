using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Pokedex.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public int NationalNo { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Species { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double FemaleGenderRatio { get; set; }
        public double MaleGenderRatio { get; set; }
        public string? Image { get; set; }
        //Stats
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }

        public ICollection<Type>? Types { get; set; }
        public ICollection<Ability>? Abilities { get; set; }
    }
}
