using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pokedex.Models;
using Pokedex.Models.Data;
using static Azure.Core.HttpHeader;

namespace Pokedex.Controllers.Pokedex
{
    public class TypesController : Controller
    {
        private readonly PokedexContext _context;
        const string _view = "Views/Admin/Pokedex/Types/";
        const string _url = "/Admin/PokedexTypes";

        public TypesController(PokedexContext context)
        {
            _context = context;
        }

        [Route(_url)]
        public async Task<IActionResult> Index()
        {
            return _context.Types != null ?
                        View(_view + "Index.cshtml", await _context.Types.ToListAsync()) :
                        Problem("Entity set 'PokedexContext.Types'  is null.");
        }

        [Route(_url + "/Create")]
        public IActionResult Create()
        {
            return View(_view + "Create.cshtml");
        }

        [Route(_url + "/Create"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content")] Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(_view + "Create.cshtml", type);
        }

        [Route(_url + "/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null) return NotFound();

            var @type = await _context.Types.FindAsync(id);
            if (@type == null) return NotFound();

            return View(_view + "Edit.cshtml", @type);
        }

        [Route(_url + "/Edit/{id}"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content")] Models.Type @type)
        {
            if (id != @type.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(_view + "Edit.cshtml", @type);
        }

        [Route(_url + "/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Types == null) return NotFound();

            var @type = await _context.Types.FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null) return NotFound();

            return View(_view + "Delete.cshtml", @type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'PokedexContext.Types'  is null.");
            }
            var @type = await _context.Types.FindAsync(id);
            if (@type != null)
            {
                _context.Types.Remove(@type);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
            return (_context.Types?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool TypeExists(string content)
        {
            return (_context.Types?.Any(e => e.Content.ToLower() == content.ToLower())).GetValueOrDefault();
        }
    }
}
