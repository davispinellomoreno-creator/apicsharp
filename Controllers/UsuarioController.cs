using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apicsharp.Data;
using apicsharp.DTOs;

namespace apicsharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        return Ok(new UsuarioResponseDto(usuario.Id, usuario.Nome, usuario.Email));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, UsuarioRequestDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        await _context.SaveChangesAsync();

        return Ok(new UsuarioResponseDto(usuario.Id, usuario.Nome, usuario.Email));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}