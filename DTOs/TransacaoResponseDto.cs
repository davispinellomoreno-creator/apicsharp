using apicsharp.Models;

namespace apicsharp.DTOs;
public record TransacaoResponseDto(
int Id,
string Descricao,
decimal Valor,
DateOnly Data,
string NomeCategoria,
TipoCategoria TipoCategoria
);
