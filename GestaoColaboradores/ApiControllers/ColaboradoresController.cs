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
    public class ColaboradoresController : ControllerBase
    {
        private readonly GestaoColaboradoresContext _context;

        public ColaboradoresController(GestaoColaboradoresContext context)
        {
            _context = context;
        }

        // GET: api/Colaboradores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaborador()
        {
          if (_context.Colaborador == null)
          {
              return NotFound();
          }
            return await _context.Colaborador.ToListAsync();
        }

        // GET: api/Colaboradores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> GetColaborador(int? id)
        {
          if (_context.Colaborador == null)
          {
              return NotFound();
          }
            var colaborador = await _context.Colaborador.FindAsync(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return colaborador;
        }

        // PUT: api/Colaboradores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaborador(int? id, Colaborador colaborador)
        {
            if (id != colaborador.IdColaborador)
            {
                return BadRequest();
            }

            _context.Entry(colaborador).State = EntityState.Modified;

            try
            {
                var resultado = await VerificarUsuarioEUnidade(colaborador);

                if (resultado != null)
                {
                    return Problem($"Não foi encontrado um {(resultado == "IdUsuario" ? "usuário" : "cadastro de unidade")} com esse {(resultado == "IdUsuario" ? "IdUsuario" : "Código Unidade")} na base.");
                }
                else
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColaboradorExists(id))
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

        // POST: api/Colaboradores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Colaborador>> PostColaborador(Colaborador colaborador)
        {
            if (_context.Colaborador == null)
            {
                return Problem("Entity set 'GestaoColaboradoresContext.Colaborador' is null.");
            }

            var resultado = await VerificarUsuarioEUnidade(colaborador);
            var unidadeAtiva = _context.Unidade.FirstOrDefault(m => m.CodigoUnidade == colaborador.CodigoUnidade);

            if (resultado != null)
            {
                return Problem($"Não foi encontrado um {(resultado == "IdUsuario" ? "usuário" : "cadastro de unidade")} com esse {(resultado == "IdUsuario" ? "IdUsuario" : "Código Unidade")} na base.");
                
            }
            else if (!unidadeAtiva.UnidadeAtiva)
            {
                return Problem($"A unidade '{colaborador.CodigoUnidade}' está desativada e não permite a inclusão de novos colaboradores.");
                
            }

            _context.Colaborador.Add(colaborador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColaborador", new { id = colaborador.IdColaborador }, colaborador);
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

        // DELETE: api/Colaboradores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaborador(int? id)
        {
            if (_context.Colaborador == null)
            {
                return NotFound();
            }
            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }

            _context.Colaborador.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColaboradorExists(int? id)
        {
            return (_context.Colaborador?.Any(e => e.IdColaborador == id)).GetValueOrDefault();
        }
    }
}
