using Microsoft.AspNetCore.Mvc;
using apicsharp.DTOs;
using apicsharp.Services;
namespace apicsharp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
// TODO: trocar pelo Id do usuário vindo do token JWT autenticado
private const int UsuarioIdFixo = 1;

private readonly ITransacaoService _service;
public TransacoesController(ITransacaoService service)
{
_service = service;
}
[HttpGet]
public async Task<IActionResult> Listar()
{
var transacoes = await _service.ListarAsync(UsuarioIdFixo);
return Ok(transacoes);
}
[HttpGet("{id}")]
public async Task<IActionResult> ObterPorId(int id)
{
var transacao = await _service.ObterPorIdAsync(id, UsuarioIdFixo);
return transacao is null ? NotFound() : Ok(transacao);
}
[HttpPost]
public async Task<IActionResult> Criar(TransacaoRequestDto dto)
{
try
{
var criada = await _service.CriarAsync(dto, UsuarioIdFixo);
return CreatedAtAction(nameof(ObterPorId), new { id =
criada.Id }, criada);
}
catch (CategoriaNaoEncontradaException ex)
{
return BadRequest(new { erro = ex.Message });
}
}
[HttpPut("{id}")]
public async Task<IActionResult> Atualizar(int id,
TransacaoRequestDto dto)
{
try
{
var sucesso = await _service.AtualizarAsync(id, dto, UsuarioIdFixo);
return sucesso ? NoContent() : NotFound();
}
catch (CategoriaNaoEncontradaException ex)
{
return BadRequest(new { erro = ex.Message });
}
}
[HttpDelete("{id}")]
public async Task<IActionResult> Remover(int id)
{
var sucesso = await _service.RemoverAsync(id, UsuarioIdFixo);
return sucesso ? NoContent() : NotFound();
}
}
