namespace apicsharp.DTOs;
public record TransacaoRequestDto(
string Descricao,
decimal Valor,
DateOnly Data,
int CategoriaId
);