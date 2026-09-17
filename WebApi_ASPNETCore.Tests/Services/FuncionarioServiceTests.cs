using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.DataContext;
using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Models;
using WebApi_ASPNETCore.Service.FuncionarioService;
using Xunit;

namespace WebApi_ASPNETCore.Tests.Services;

public class FuncionarioServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetFuncionarios_DeveRetornarFuncionariosCadastrados()
    {
        await using var context = CreateContext();

        context.Funcionarios.AddRange(
            CriarFuncionario("Anderson", "Domingos"),
            CriarFuncionario("Maria", "Silva"));

        await context.SaveChangesAsync();

        var service = new FuncionarioService(context);

        var response = await service.GetFuncionarios(
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.NotNull(response.Dados);
        Assert.Equal(2, response.Dados.Count);
    }

    [Fact]
    public async Task GetFuncionariosById_QuandoFuncionarioExiste_DeveRetornarFuncionario()
    {
        await using var context = CreateContext();

        var funcionario = CriarFuncionario(
            "Anderson",
            "Domingos");

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();

        var service = new FuncionarioService(context);

        var response = await service.GetFuncionariosById(
            funcionario.Id,
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.NotNull(response.Dados);
        Assert.Equal(funcionario.Id, response.Dados.Id);
        Assert.Equal("Anderson", response.Dados.Nome);
        Assert.Equal("Domingos", response.Dados.Sobrenome);
    }

    [Fact]
    public async Task GetFuncionariosById_QuandoFuncionarioNaoExiste_DeveRetornarFalha()
    {
        await using var context = CreateContext();

        var service = new FuncionarioService(context);

        var response = await service.GetFuncionariosById(
            999,
            CancellationToken.None);

        Assert.False(response.Sucesso);
        Assert.Null(response.Dados);
        Assert.Equal(
            "Funcionário não foi encontrado!",
            response.Mensagem);
    }

    [Fact]
    public async Task CreateFuncionarios_DeveCadastrarNovoFuncionario()
    {
        await using var context = CreateContext();

        var service = new FuncionarioService(context);

        var request = new FuncionarioRequest
        {
            Nome = "  João  ",
            Sobrenome = "  Souza  ",
            Ativo = true
        };

        var response = await service.CreateFuncionarios(
            request,
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.NotNull(response.Dados);
        Assert.Equal("João", response.Dados.Nome);
        Assert.Equal("Souza", response.Dados.Sobrenome);
        Assert.True(response.Dados.Id > 0);

        Assert.Equal(
            1,
            await context.Funcionarios.CountAsync());
    }

    [Fact]
    public async Task UpdateFuncionarios_QuandoFuncionarioExiste_DeveAtualizarDados()
    {
        await using var context = CreateContext();

        var funcionario = CriarFuncionario(
            "Carlos",
            "Santos");

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();

        var service = new FuncionarioService(context);

        var request = new FuncionarioRequest
        {
            Nome = "Carlos",
            Sobrenome = "Oliveira",
            Ativo = true
        };

        var response = await service.UpdateFuncionarios(
            request,
            funcionario.Id,
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.NotNull(response.Dados);
        Assert.Equal(
            "Oliveira",
            response.Dados.Sobrenome);

        var atualizado = await context.Funcionarios
            .FindAsync(funcionario.Id);

        Assert.NotNull(atualizado);
        Assert.Equal(
            "Oliveira",
            atualizado.Sobrenome);
    }

    [Fact]
    public async Task InativaFuncionario_QuandoFuncionarioExiste_DeveDesativarFuncionario()
    {
        await using var context = CreateContext();

        var funcionario = CriarFuncionario(
            "Ana",
            "Costa");

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();

        var service = new FuncionarioService(context);

        var response = await service.InativaFuncionario(
            funcionario.Id,
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.NotNull(response.Dados);
        Assert.False(response.Dados.Ativo);

        var inativo = await context.Funcionarios
            .FindAsync(funcionario.Id);

        Assert.NotNull(inativo);
        Assert.False(inativo.Ativo);
    }

    [Fact]
    public async Task DeleteFuncionarios_QuandoFuncionarioExiste_DeveExcluirFuncionario()
    {
        await using var context = CreateContext();

        var funcionario = CriarFuncionario(
            "Pedro",
            "Lima");

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();

        var service = new FuncionarioService(context);

        var response = await service.DeleteFuncionarios(
            funcionario.Id,
            CancellationToken.None);

        Assert.True(response.Sucesso);
        Assert.True(response.Dados);

        var existe = await context.Funcionarios
            .AnyAsync(f => f.Id == funcionario.Id);

        Assert.False(existe);
    }

    private static FuncionarioModel CriarFuncionario(
        string nome,
        string sobrenome)
    {
        return new FuncionarioModel
        {
            Nome = nome,
            Sobrenome = sobrenome,
            Ativo = true,
            DataDeCriacao = DateTime.UtcNow,
            DataDeAlteracao = DateTime.UtcNow
        };
    }
}
