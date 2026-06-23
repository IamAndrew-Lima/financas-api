using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancasAPI.Data;
using FinancasAPI.Models;

namespace FinancasAPI.Controllers;

[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransacoesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/transacoes
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var transacoes = await _context.Transacoes.ToListAsync();
        return Ok(transacoes);
    }

    // GET: api/transacoes/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);

        if (transacao == null)
            return NotFound();

        return Ok(transacao);
    }

    // POST: api/transacoes
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return Ok(transacao);
    }

    // PUT: api/transacoes/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Transacao transacao)
    {
        var existente = await _context.Transacoes.FindAsync(id);

        if (existente == null)
            return NotFound();

        existente.Descricao = transacao.Descricao;
        existente.Valor = transacao.Valor;
        existente.Tipo = transacao.Tipo;
        existente.Categoria = transacao.Categoria;
        existente.DataMovimentacao = transacao.DataMovimentacao;

        await _context.SaveChangesAsync();

        return Ok(existente);
    }

    // DELETE: api/transacoes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);

        if (transacao == null)
            return NotFound();

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();

        return Ok();
    }
}