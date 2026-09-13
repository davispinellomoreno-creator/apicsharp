using apicsharp.DTOs;
using apicsharp.Models;
using apicsharp.Repositories;

namespace apicsharp.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _repository;

    public TransacaoService(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TransacaoResponseDto>> ListarAsync(int usuarioId)
    {
        var transacoes = await _repository.ObterTodasAsync(usuarioId);

        return transacoes.Select(t => new TransacaoResponseDto(
            t.Id, t.Descricao, t.Valor, t.Data,
            t.Categoria.Nome, t.Categoria.Tipo
        )).ToList();
    }

    public async Task<TransacaoResponseDto?> ObterPorIdAsync(int id)
    {
        var t = await _repository.ObterPorIdAsync(id);
        if (t is null) return null;

        return new TransacaoResponseDto(
            t.Id, t.Descricao, t.Valor, t.Data,
            t.Categoria.Nome, t.Categoria.Tipo
        );
    }

    public async Task<TransacaoResponseDto> CriarAsync(TransacaoRequestDto dto, int usuarioId)
    {
        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Data = dto.Data,
            CategoriaId = dto.CategoriaId,
            UsuarioId = usuarioId
        };

        await _repository.AdicionarAsync(transacao);
        await _repository.SalvarAsync();

        return new TransacaoResponseDto(
            transacao.Id, transacao.Descricao, transacao.Valor,
            transacao.Data, "", ""
        );
    }

    public async Task<bool> AtualizarAsync(int id, TransacaoRequestDto dto)
    {
        var transacao = await _repository.ObterPorIdAsync(id);
        if (transacao is null) return false;

        transacao.Descricao = dto.Descricao;
        transacao.Valor = dto.Valor;
        transacao.Data = dto.Data;
        transacao.CategoriaId = dto.CategoriaId;

        _repository.Atualizar(transacao);
        return await _repository.SalvarAsync();
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var transacao = await _repository.ObterPorIdAsync(id);
        if (transacao is null) return false;

        _repository.Remover(transacao);
        return await _repository.SalvarAsync();
    }
}