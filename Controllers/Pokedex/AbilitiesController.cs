using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pokedex.Models;
using Pokedex.Models.Data;

namespace Pokedex.Controllers.Pokedex
{
    public class AbilitiesController : Controller
    {
        private readonly PokedexContext _context;
        const string _view = "Views/Admin/Pokedex/Abilities/";
        const string _url = "/Admin/PokedexAbilities";

        public AbilitiesController(PokedexContext context)
        {
            _context = context;
        }

        [Route(_url)]
        public async Task<IActionResult> Index()
        {
            return _context.Abilities != null ?
                        View(_view + "Index.cshtml", await _context.Abilities.ToListAsync()) :
                        Problem("Entity set 'PokedexContext.Abilities'  is null.");
        }

        [Route(_url + "/Create")]
        public IActionResult Create()
        {
            return View(_view + "Create.cshtml");
        }

        [Route(_url + "/Create"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Effect,Type")] Ability ability)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(_view + "Create.cshtml", ability);
        }

        [Route(_url + "/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Abilities == null) return NotFound();

            var ability = await _context.Abilities.FindAsync(id);
            if (ability == null) return NotFound();

            return View(_view + "Edit.cshtml", ability);
        }

        [Route(_url + "/Edit/{id}"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Effect,Type")] Ability ability)
        {
            if (id != ability.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbilityExists(ability.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_view + "Edit.cshtml", ability);
        }

        [Route(_url + "/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Abilities == null)
            {
                return NotFound();
            }

            var ability = await _context.Abilities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ability == null)
            {
                return NotFound();
            }

            return View(_view + "Delete.cshtml", ability);
        }

        // POST: Abilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Abilities == null)
            {
                return Problem("Entity set 'PokedexContext.Abilities'  is null.");
            }
            var ability = await _context.Abilities.FindAsync(id);
            if (ability != null)
            {
                _context.Abilities.Remove(ability);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbilityExists(int id)
        {
            return (_context.Abilities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
