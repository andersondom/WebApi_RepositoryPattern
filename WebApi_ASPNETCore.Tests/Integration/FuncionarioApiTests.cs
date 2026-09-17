using System.Net;
using System.Net.Http.Json;
using WebApi_ASPNETCore.DTOs.Funcionario;
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
    public async Task GetFuncionario_QuandoNaoExiste_DeveRetornar404()
    {
        var response = await _client.GetAsync(
            "/api/Funcionario/999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task PostFuncionario_ComDadosValidos_DeveRetornar201()
    {
        var request = new FuncionarioRequest
        {
            Nome = "Anderson",
            Sobrenome = "Domingos",
            Ativo = true
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Funcionario",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var resultado = await response.Content
            .ReadFromJsonAsync<
                ServiceResponse<FuncionarioResponse>>();

        Assert.NotNull(resultado);
        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
        Assert.True(resultado.Dados.Id > 0);
        Assert.Equal(
            "Anderson",
            resultado.Dados.Nome);
    }

    [Fact]
    public async Task PostFuncionario_ComNomeInvalido_DeveRetornar400()
    {
        var request = new FuncionarioRequest
        {
            Nome = "",
            Sobrenome = "Domingos",
            Ativo = true
        };

        var response = await _client.PostAsJsonAsync(
            "/api/Funcionario",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
