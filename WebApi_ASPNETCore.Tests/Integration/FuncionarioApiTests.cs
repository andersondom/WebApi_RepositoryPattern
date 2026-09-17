using System.Net;
using System.Net.Http.Json;
using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Enums;
using WebApi_ASPNETCore.Models;
using Xunit;

namespace WebApi_ASPNETCore.Tests.Integration;

public class FuncionarioApiTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FuncionarioApiTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetFuncionarioById_QuandoNaoExiste_DeveRetornarNotFound()
    {
        var response =
            await _client.GetAsync("/api/funcionarios/999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateFuncionario_ComDadosValidos_DeveRetornarCreated()
    {
        var request = new FuncionarioRequest
        {
            Nome = "Anderson",
            Sobrenome = "Teste",
            Departamento = (DepartamentoEnum)1,
            Turno = (TurnoEnum)1,
            Ativo = true
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/funcionarios",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var resultado =
            await response.Content
                .ReadFromJsonAsync<ServiceResponse<FuncionarioResponse>>();

        Assert.NotNull(resultado);
        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
        Assert.True(resultado.Dados.Id > 0);
        Assert.Equal("Anderson", resultado.Dados.Nome);
    }

    [Fact]
    public async Task CreateFuncionario_ComNomeInvalido_DeveRetornarBadRequest()
    {
        var request = new FuncionarioRequest
        {
            Nome = "",
            Sobrenome = "Teste",
            Departamento = (DepartamentoEnum)1,
            Turno = (TurnoEnum)1,
            Ativo = true
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/funcionarios",
                request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task HealthCheck_DeveRetornarOk()
    {
        var response =
            await _client.GetAsync("/health");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
}
