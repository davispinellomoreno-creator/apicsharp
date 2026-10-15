using System.ComponentModel.DataAnnotations;
using apicsharp.Models;

namespace apicsharp.DTOs;
public record CategoriaRequestDto(
[Required, StringLength(100, MinimumLength = 1)] string Nome,
[EnumDataType(typeof(TipoCategoria))] TipoCategoria Tipo
);
public record CategoriaResponseDto(
int Id,
string Nome,
TipoCategoria Tipo
);
