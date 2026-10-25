using apicsharp.DTOs;
namespace apicsharp.Services;
public interface ITransacaoService
{
Task<List<TransacaoResponseDto>> ListarAsync(int usuarioId);
Task<TransacaoResponseDto?> ObterPorIdAsync(int id, int usuarioId);
Task<TransacaoResponseDto> CriarAsync(TransacaoRequestDto dto,
int usuarioId);
Task<bool> AtualizarAsync(int id, TransacaoRequestDto dto, int usuarioId);
Task<bool> RemoverAsync(int id, int usuarioId);
}
