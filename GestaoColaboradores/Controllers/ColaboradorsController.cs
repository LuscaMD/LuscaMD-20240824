using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoColaboradores.Context;
using GestaoColaboradores.Models.Entities;

namespace GestaoColaboradores.Controllers
{
    public class ColaboradorsController : Controller
    {
        private readonly GestaoColaboradoresContext _context;

        public ColaboradorsController(GestaoColaboradoresContext context)
        {
            _context = context;
        }

        // GET: Colaboradors
        public async Task<IActionResult> Index()
        {
              return _context.Colaborador != null ? 
                          View(await _context.Colaborador.ToListAsync()) :
                          Problem("Entity set 'GestaoColaboradoresContext.Colaborador'  is null.");
        }

        // GET: Colaboradors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.IdColaborador == id);
            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        // GET: Colaboradors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colaboradors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdColaborador,Nome,CodigoUnidade,IdUsuario")] Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                var resultado = await VerificarUsuarioEUnidade(colaborador);
                var unidadeAtiva = _context.Unidade.FirstOrDefault(m => m.CodigoUnidade == colaborador.CodigoUnidade);

                if (resultado != null)
                {
                    ModelState.AddModelError(resultado, $"Não foi encontrado um {(resultado == "IdUsuario" ? "usuário" : "cadastro de unidade")} com esse {(resultado == "IdUsuario" ? "IdUsuario" : "Código Unidade")} na base.");
                    return View(colaborador);
                }
                else if (!unidadeAtiva.UnidadeAtiva)
                {
                    ModelState.AddModelError("IdUnidade", $"A unidade '{colaborador.CodigoUnidade}' está desativada e não permite a inclusão de novos colaboradores.");
                    return View(colaborador);
                }
                else
                {
                    _context.Add(colaborador);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(colaborador);
        }

        private async Task<string> VerificarUsuarioEUnidade(Colaborador colaborador)
        {
            var usuarioBusca = await _context.Usuario.FirstOrDefaultAsync(m => m.IdUsuario == colaborador.IdUsuario);
            var unidadeBusca = await _context.Unidade.FirstOrDefaultAsync(m => m.CodigoUnidade == colaborador.CodigoUnidade);

            if (usuarioBusca == null)
            {
                return "CodigoUnidade";
            }
            else if (unidadeBusca == null)
            {
                return "IdUnidade";
            }

            return null;
        }

        // GET: Colaboradors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return View(colaborador);
        }

        // POST: Colaboradors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("IdColaborador,Nome,CodigoUnidade,IdUsuario")] Colaborador colaborador)
        {
            if (id != colaborador.IdColaborador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var resultado = await VerificarUsuarioEUnidade(colaborador);

                    if (resultado != null)
                    {
                        ModelState.AddModelError(resultado, $"Não foi encontrado um {(resultado == "IdUsuario" ? "usuário" : "cadastro de unidade")} com esse {(resultado == "IdUsuario" ? "IdUsuario" : "Código Unidade")} na base.");
                        return View(colaborador);
                    }
                    else
                    {
                        _context.Update(colaborador);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorExists(colaborador.IdColaborador))
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
            return View(colaborador);
        }

        // GET: Colaboradors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Colaborador == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.IdColaborador == id);
            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        // POST: Colaboradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Colaborador == null)
            {
                return Problem("Entity set 'GestaoColaboradoresContext.Colaborador'  is null.");
            }
            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador != null)
            {
                _context.Colaborador.Remove(colaborador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColaboradorExists(int? id)
        {
          return (_context.Colaborador?.Any(e => e.IdColaborador == id)).GetValueOrDefault();
        }
    }
}
