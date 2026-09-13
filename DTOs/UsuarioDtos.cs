namespace apicsharp.DTOs;
public record UsuarioRequestDto(
string Nome,
string Email,
string Senha
);
public record UsuarioResponseDto(
int Id,
string Nome,
string Email
);