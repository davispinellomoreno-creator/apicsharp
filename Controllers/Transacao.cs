using Microsoft.AspNetCore.Mvc;
using apicsharp.DTOs;
using apicsharp.Services;
namespace apicsharp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
private readonly ITransacaoService _service;
public TransacoesController(ITransacaoService service)
{
_service = service;
}
[HttpGet]
public async Task<IActionResult> Listar()
{
var usuarioId = 1; // viria do token JWT autenticado
var transacoes = await _service.ListarAsync(usuarioId);
return Ok(transacoes);
}
[HttpGet("{id}")]
public async Task<IActionResult> ObterPorId(int id)
{
var transacao = await _service.ObterPorIdAsync(id);
return transacao is null ? NotFound() : Ok(transacao);
}
[HttpPost]
public async Task<IActionResult> Criar(TransacaoRequestDto dto)
{
var usuarioId = 1; // viria do token JWT autenticado
var criada = await _service.CriarAsync(dto, usuarioId);
return CreatedAtAction(nameof(ObterPorId), new { id =
criada.Id }, criada);
}
[HttpPut("{id}")]
public async Task<IActionResult> Atualizar(int id,
TransacaoRequestDto dto)
{
var sucesso = await _service.AtualizarAsync(id, dto);
return sucesso ? NoContent() : NotFound();
}
[HttpDelete("{id}")]
public async Task<IActionResult> Remover(int id)
{
var sucesso = await _service.RemoverAsync(id);
return sucesso ? NoContent() : NotFound();
}
}