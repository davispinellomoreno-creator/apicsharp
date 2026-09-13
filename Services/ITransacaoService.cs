using apicsharp.DTOs;
namespace apicsharp.Services;
public interface ITransacaoService
{
Task<List<TransacaoResponseDto>> ListarAsync(int usuarioId);
Task<TransacaoResponseDto?> ObterPorIdAsync(int id);
Task<TransacaoResponseDto> CriarAsync(TransacaoRequestDto dto,
int usuarioId);
Task<bool> AtualizarAsync(int id, TransacaoRequestDto dto);
Task<bool> RemoverAsync(int id);
}
