using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apicsharp.Data;
using apicsharp.DTOs;
using apicsharp.Models;

namespace apicsharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuariosController(AppDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        return Ok(ParaDto(usuario));
    }

    [HttpPost]
    public async Task<IActionResult> Criar(UsuarioRequestDto dto)
    {
        if (await EmailEmUsoAsync(dto.Email))
            return Conflict(new { erro = "E-mail já cadastrado." });

        var usuario = new Usuario { Nome = dto.Nome, Email = dto.Email };
        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, ParaDto(usuario));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, UsuarioRequestDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        if (await EmailEmUsoAsync(dto.Email, ignorarId: id))
            return Conflict(new { erro = "E-mail já cadastrado." });

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);
        await _context.SaveChangesAsync();

        return Ok(ParaDto(usuario));
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

    private Task<bool> EmailEmUsoAsync(string email, int? ignorarId = null) =>
        _context.Usuarios.AnyAsync(u => u.Email == email && u.Id != ignorarId);

    private static UsuarioResponseDto ParaDto(Usuario u) => new(u.Id, u.Nome, u.Email);
}
