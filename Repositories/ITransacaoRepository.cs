using apicsharp.Models;

namespace apicsharp.Repositories;

public interface ITransacaoRepository
{
    Task<List<Transacao>> ObterTodasAsync(int usuarioId);
    Task<Transacao?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Transacao transacao);
    void Atualizar(Transacao transacao);
    void Remover(Transacao transacao);
    Task<bool> SalvarAsync();
}