using System.ComponentModel.DataAnnotations;

namespace apicsharp.DTOs;
public record UsuarioRequestDto(
[Required, StringLength(100, MinimumLength = 1)] string Nome,
[Required, EmailAddress, StringLength(200)] string Email,
[Required, StringLength(100, MinimumLength = 6)] string Senha
);
public record UsuarioResponseDto(
int Id,
string Nome,
string Email
);
