using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<ServiceResponse<List<FuncionarioModel>>>> GetFuncionarios()
    {
        var response = await _funcionarioService.GetFuncionarios();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResponse<FuncionarioModel>>> GetFuncionarioById(int id)
    {
        var response = await _funcionarioService.GetFuncionariosById(id);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<FuncionarioModel>>>> CreateFuncionario(
        FuncionarioModel model)
    {
        var response = await _funcionarioService.CreateFuncionarios(model);

        return CreatedAtAction(
            nameof(GetFuncionarioById),
            new { id = model.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<FuncionarioModel>>>> UpdateFuncionario(
        int id,
        FuncionarioModel model)
    {
        var response = await _funcionarioService.UpdateFuncionarios(model, id);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<ActionResult<ServiceResponse<List<FuncionarioModel>>>> DeactivateFuncionario(
        int id)
    {
        var response = await _funcionarioService.InativaFuncionario(id);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<FuncionarioModel>>>> DeleteFuncionario(int id)
    {
        var response = await _funcionarioService.DeleteFuncionarios(id);

        if (!response.Sucesso)
            return NotFound(response);

        return Ok(response);
    }
}