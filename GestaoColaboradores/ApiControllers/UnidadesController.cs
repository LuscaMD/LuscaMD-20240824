using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoColaboradores.Context;
using GestaoColaboradores.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GestaoColaboradores.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly GestaoColaboradoresContext _context;

        public UnidadesController(GestaoColaboradoresContext context)
        {
            _context = context;
        }

        // GET: api/Unidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidade>>> GetUnidade()
        {
          if (_context.Unidade == null)
          {
              return NotFound();
          }
            return await _context.Unidade.ToListAsync();
        }

        // GET: api/Unidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unidade>> GetUnidade(int id)
        {
          if (_context.Unidade == null)
          {
              return NotFound();
          }
            var unidade = await _context.Unidade.FindAsync(id);

            if (unidade == null)
            {
                return NotFound();
            }

            return unidade;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidade(int id, Unidade unidade)
        {
            if (id != unidade.IdUnidade)
            {
                return BadRequest();
            }

            _context.Entry(unidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Unidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unidade>> PostUnidade(Unidade unidade)
        {
            if (_context.Unidade == null)
            {
                return Problem("Entity set 'GestaoColaboradoresContext.Unidade'  is null.");
            }

            var usuarioBusca = await _context.Unidade.FirstOrDefaultAsync(m => m.CodigoUnidade == unidade.CodigoUnidade);

            if (usuarioBusca != null)
            {
                return Problem("Já existe uma unidade com este código unidade na base.");
            }

            _context.Unidade.Add(unidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidade", new { id = unidade.IdUnidade }, unidade);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidade(int id)
        {
            if (_context.Unidade == null)
            {
                return NotFound();
            }
            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade == null)
            {
                return NotFound();
            }

            _context.Unidade.Remove(unidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadeExists(int id)
        {
            return (_context.Unidade?.Any(e => e.IdUnidade == id)).GetValueOrDefault();
        }
    }
}
