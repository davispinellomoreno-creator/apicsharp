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
        return transacoes.Select(ParaDto).ToList();
    }

    public async Task<TransacaoResponseDto?> ObterPorIdAsync(int id, int usuarioId)
    {
        var t = await _repository.ObterPorIdAsync(id, usuarioId);
        return t is null ? null : ParaDto(t);
    }

    /// <exception cref="CategoriaNaoEncontradaException">Se a categoria informada não existir.</exception>
    public async Task<TransacaoResponseDto> CriarAsync(TransacaoRequestDto dto, int usuarioId)
    {
        await GarantirCategoriaExisteAsync(dto.CategoriaId);

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

        // Busca de novo para trazer a Categoria preenchida na resposta
        var criada = await _repository.ObterPorIdAsync(transacao.Id, usuarioId);
        return ParaDto(criada!);
    }

    /// <exception cref="CategoriaNaoEncontradaException">Se a categoria informada não existir.</exception>
    public async Task<bool> AtualizarAsync(int id, TransacaoRequestDto dto, int usuarioId)
    {
        var transacao = await _repository.ObterPorIdAsync(id, usuarioId);
        if (transacao is null) return false;

        if (transacao.CategoriaId != dto.CategoriaId)
            await GarantirCategoriaExisteAsync(dto.CategoriaId);

        transacao.Descricao = dto.Descricao;
        transacao.Valor = dto.Valor;
        transacao.Data = dto.Data;
        transacao.CategoriaId = dto.CategoriaId;

        // A entidade já está sendo rastreada pelo EF, basta salvar
        await _repository.SalvarAsync();
        return true;
    }

    public async Task<bool> RemoverAsync(int id, int usuarioId)
    {
        var transacao = await _repository.ObterPorIdAsync(id, usuarioId);
        if (transacao is null) return false;

        _repository.Remover(transacao);
        return await _repository.SalvarAsync();
    }

    private async Task GarantirCategoriaExisteAsync(int categoriaId)
    {
        if (!await _repository.CategoriaExisteAsync(categoriaId))
            throw new CategoriaNaoEncontradaException(categoriaId);
    }

    private static TransacaoResponseDto ParaDto(Transacao t) => new(
        t.Id, t.Descricao, t.Valor, t.Data,
        t.Categoria.Nome, t.Categoria.Tipo
    );
}
