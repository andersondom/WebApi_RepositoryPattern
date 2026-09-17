using Microsoft.AspNetCore.Mvc;
using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Models;
using WebApi_ASPNETCore.Service.FuncionarioService;

namespace WebApi_ASPNETCore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly IFuncionarioInterface _funcionarioService;

    public FuncionarioController(IFuncionarioInterface funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<FuncionarioResponse>>>> GetFuncionarios(
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.GetFuncionarios(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResponse<FuncionarioResponse>>> GetFuncionarioById(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.GetFuncionariosById(
            id,
            cancellationToken);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FuncionarioResponse>>> CreateFuncionario(
        [FromBody] FuncionarioRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.CreateFuncionarios(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetFuncionarioById),
            new { id = response.Dados!.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceResponse<FuncionarioResponse>>> UpdateFuncionario(
        int id,
        [FromBody] FuncionarioRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.UpdateFuncionarios(
            request,
            id,
            cancellationToken);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<ActionResult<ServiceResponse<FuncionarioResponse>>> DeactivateFuncionario(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.InativaFuncionario(
            id,
            cancellationToken);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteFuncionario(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _funcionarioService.DeleteFuncionarios(
            id,
            cancellationToken);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }
}