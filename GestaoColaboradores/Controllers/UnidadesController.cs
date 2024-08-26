using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoColaboradores.Context;
using GestaoColaboradores.Models.Entities;
using GestaoColaboradores.Models.ViewModel;

namespace GestaoColaboradores.Controllers
{
    public class UnidadesController : Controller
    {
        private readonly GestaoColaboradoresContext _context;

        public UnidadesController(GestaoColaboradoresContext context)
        {
            _context = context;
        }

        // GET: Unidades
        public async Task<IActionResult> Index()
        {
            var unidades = await _context.Unidade.Include(x => x.Colaboradores).ToListAsync();

            var viewModel = unidades.Select(x => new UnidadeColaboradoresViewModel
            {
                Unidade = x,
                Colaboradores = x.Colaboradores.ToList()
            }).ToList();

            return viewModel != null ? 
                        View(viewModel) :
                        Problem("Entity set 'GestaoColaboradoresContext.UnidadeColaboradoresViewModel' is null.");
        }

        // GET: Unidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Unidade == null)
            {
                return NotFound();
            }

            var unidade = await _context.Unidade
                .FirstOrDefaultAsync(m => m.IdUnidade == id);
            if (unidade == null)
            {
                return NotFound();
            }

            return View(unidade);
        }

        // GET: Unidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUnidade,CodigoUnidade,NomeUnidade,UnidadeAtiva")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                var usuarioBusca = await _context.Unidade.FirstOrDefaultAsync(m => m.CodigoUnidade == unidade.CodigoUnidade);

                if (usuarioBusca != null)
                {
                    ModelState.AddModelError("CodigoUnidade", "Já existe uma unidade com este código unidade na base.");
                    return View(unidade);
                }
                else
                {
                    _context.Add(unidade);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return View(unidade);
        }

        // GET: Unidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Unidade == null)
            {
                return NotFound();
            }

            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade == null)
            {
                return NotFound();
            }
            return View(unidade);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUnidade,CodigoUnidade,NomeUnidade,UnidadeAtiva")] Unidade unidade)
        {
            if (id != unidade.IdUnidade)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadeExists(unidade.IdUnidade))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(unidade);
        }

        // GET: Unidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Unidade == null)
            {
                return NotFound();
            }

            var unidade = await _context.Unidade
                .FirstOrDefaultAsync(m => m.IdUnidade == id);
            if (unidade == null)
            {
                return NotFound();
            }

            return View(unidade);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Unidade == null)
            {
                return Problem("Entity set 'GestaoColaboradoresContext.Unidade'  is null.");
            }
            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade != null)
            {
                _context.Unidade.Remove(unidade);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnidadeExists(int id)
        {
          return (_context.Unidade?.Any(e => e.IdUnidade == id)).GetValueOrDefault();
        }
    }
}
