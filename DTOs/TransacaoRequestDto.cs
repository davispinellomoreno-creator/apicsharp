using System.ComponentModel.DataAnnotations;

namespace apicsharp.DTOs;
public record TransacaoRequestDto(
[Required, StringLength(200, MinimumLength = 1)] string Descricao,
[Range(typeof(decimal), "0.01", "9999999999999999.99")] decimal Valor,
DateOnly Data,
[Range(1, int.MaxValue)] int CategoriaId
);
