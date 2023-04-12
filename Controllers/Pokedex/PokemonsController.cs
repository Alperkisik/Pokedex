using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pokedex.Models;
using Pokedex.Models.Data;

namespace Pokedex.Controllers.Pokedex
{
    public class PokemonsController : Controller
    {
        private readonly PokedexContext _context;
        const string _view = "Views/Admin/Pokedex/Pokemons/";
        const string _url = "/Admin/PokedexPokemons";

        public PokemonsController(PokedexContext context)
        {
            _context = context;
        }

        [Route(_url)]
        public async Task<IActionResult> Index()
        {
            List<Pokemon> pokemons = _context.Pokemons.Include(o => o.Types).Include(a => a.Abilities).ToList();
            return _context.Pokemons != null ?
                        View(_view + "Index.cshtml", await _context.Pokemons.Include(o => o.Types).Include(a => a.Abilities).ToListAsync()) :
                        Problem("Entity set 'PokedexContext.Pokemons'  is null.");
        }


        [Route(_url + "/Create")]
        public IActionResult Create()
        {
            ViewBag.Types = _context.Types.ToList();
            ViewBag.Abilities = _context.Abilities.ToList();

            return View(_view + "Create.cshtml");
        }

        [Route(_url + "/Create"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NationalNo,Name,Description,Species,Height,Weight,FemaleGenderRatio,MaleGenderRatio,HP,Attack,Defense,SpecialAttack,SpecialDefense,Speed,Types,Abilities")] Pokemon pokemon, IFormFile uploadedImage)
        {
            string extent = "", path = "", _fileName = "";

            if (uploadedImage != null)
            {
                extent = Path.GetExtension(uploadedImage.FileName);

                _fileName = pokemon.Name;

                if (extent == ".png" || extent == ".jpeg" || extent == ".jpg")
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Pokemons", _fileName + extent);
                    pokemon.Image = pokemon.Name + Path.GetExtension(uploadedImage.FileName);
                }
            }

            if (pokemon.Types != null)
            {
                if (pokemon.Types.Count > 0)
                {
                    ICollection<Models.Type> Types = new List<Models.Type>();

                    foreach (var type in pokemon.Types)
                    {
                        Types.Add(_context.Types.Find(type.Id));
                    }
                    pokemon.Types = Types;
                }
            }
            
            if (pokemon.Abilities != null)
            {
                if (pokemon.Abilities.Count > 0)
                {
                    ICollection<Ability> Abilities = new List<Ability>();

                    foreach (var ability in pokemon.Abilities)
                    {
                        Abilities.Add(_context.Abilities.Find(ability.Id));
                    }
                    pokemon.Abilities = Abilities;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(pokemon);
                await _context.SaveChangesAsync();

                try
                {
                    await uploadedImage.CopyToAsync(new FileStream(path, FileMode.Create));
                }
                catch (Exception)
                {
                    await uploadedImage.CopyToAsync(new FileStream(path, FileMode.Append));
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Types = _context.Types.ToList();
            ViewBag.Abilities = _context.Abilities.ToList();

            return View(_view + "Create.cshtml", pokemon);
        }

        [Route(_url + "/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Types = _context.Types.ToList();
            ViewBag.Abilities = _context.Abilities.ToList();

            if (id == null || _context.Pokemons == null) return NotFound();

            var pokemon = await _context.Pokemons.Where(i => i.Id == id).Include(o => o.Types).Include(a => a.Abilities).FirstOrDefaultAsync();
            if (pokemon == null) return NotFound();

            return View(_view + "Edit.cshtml", pokemon);
        }

        [Route(_url + "/Edit"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NationalNo,Name,Description,Species,Height,Weight,FemaleGenderRatio,MaleGenderRatio,Image,HP,Attack,Defense,SpecialAttack,SpecialDefense,Speed,Types,Abilities")] Pokemon pokemon, IFormFile uploadedImage)
        {
            if (id != pokemon.Id) return NotFound();

            string extent = "", path = "", _fileName = "";

            if (uploadedImage != null)
            {
                extent = Path.GetExtension(uploadedImage.FileName);

                _fileName = pokemon.Name;

                if (extent == ".png" || extent == ".jpeg" || extent == ".jpg")
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Pokemons", _fileName + extent);
                    pokemon.Image = pokemon.Name + Path.GetExtension(uploadedImage.FileName);
                }
            }

            if (pokemon.Types != null)
            {
                if (pokemon.Types.Count > 0)
                {
                    ICollection<Models.Type> Types = new List<Models.Type>();

                    foreach (var type in pokemon.Types)
                    {
                        Types.Add(_context.Types.Find(type.Id));
                    }
                    pokemon.Types = Types;
                }
            }

            if (pokemon.Abilities != null)
            {
                if (pokemon.Abilities.Count > 0)
                {
                    ICollection<Ability> Abilities = new List<Ability>();

                    foreach (var ability in pokemon.Abilities)
                    {
                        Abilities.Add(_context.Abilities.Find(ability.Id));
                    }
                    pokemon.Abilities = Abilities;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pokemon);
                    await _context.SaveChangesAsync();
                    await uploadedImage.CopyToAsync(new FileStream(path, FileMode.Append));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PokemonExists(pokemon.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Types = _context.Types.ToList();
            ViewBag.Abilities = _context.Abilities.ToList();

            return View(_view + "Edit.cshtml", pokemon);
        }

        [Route(_url + "/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pokemons == null)
            {
                return NotFound();
            }

            var pokemon = await _context.Pokemons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }

            return View(pokemon);
        }

        // POST: Pokemons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pokemons == null)
            {
                return Problem("Entity set 'PokedexContext.Pokemons'  is null.");
            }
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon != null)
            {
                _context.Pokemons.Remove(pokemon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PokemonExists(int id)
        {
            return (_context.Pokemons?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Route(_url + "/GetPokemonTypes")]
        public IActionResult GetPokemonTypes()
        {
            var types = _context.Types.ToList();
            return Json(types);
        }

        [Route(_url + "/GetPokemonAbilities")]
        public IActionResult GetPokemonAbilities()
        {
            var abilities = _context.Abilities.ToList();
            return Json(abilities);
        }
    }
}
