using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apicsharp.Data;
using apicsharp.DTOs;
using apicsharp.Models;

namespace apicsharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _context.Categorias
            .Select(c => new CategoriaResponseDto(c.Id, c.Nome, c.Tipo))
            .ToListAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        return categoria is null ? NotFound() : Ok(ParaDto(categoria));
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CategoriaRequestDto dto)
    {
        var categoria = new Categoria { Nome = dto.Nome, Tipo = dto.Tipo };
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, ParaDto(categoria));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, CategoriaRequestDto dto)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        categoria.Nome = dto.Nome;
        categoria.Tipo = dto.Tipo;
        await _context.SaveChangesAsync();

        return Ok(ParaDto(categoria));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        if (await _context.Transacoes.AnyAsync(t => t.CategoriaId == id))
            return Conflict(new { erro = "Categoria possui transações e não pode ser removida." });

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static CategoriaResponseDto ParaDto(Categoria c) => new(c.Id, c.Nome, c.Tipo);
}
