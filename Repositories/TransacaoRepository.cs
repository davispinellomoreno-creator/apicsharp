using Microsoft.EntityFrameworkCore;
using apicsharp.Data;
using apicsharp.Models;

namespace apicsharp.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transacao>> ObterTodasAsync(int usuarioId)
    {
        return await _context.Transacoes
            .Include(t => t.Categoria)
            .Where(t => t.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Transacao?> ObterPorIdAsync(int id, int usuarioId)
    {
        return await _context.Transacoes
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
    }

    public async Task<bool> CategoriaExisteAsync(int categoriaId)
    {
        return await _context.Categorias.AnyAsync(c => c.Id == categoriaId);
    }

    public async Task AdicionarAsync(Transacao transacao)
    {
        await _context.Transacoes.AddAsync(transacao);
    }

    public void Remover(Transacao transacao)
    {
        _context.Transacoes.Remove(transacao);
    }

    public async Task<bool> SalvarAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
