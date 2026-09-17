using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.DataContext;
using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Models;

namespace WebApi_ASPNETCore.Service.FuncionarioService;

public class FuncionarioService : IFuncionarioInterface
{
    private readonly ApplicationDbContext _context;

    public FuncionarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<FuncionarioResponse>>> GetFuncionarios(
        CancellationToken cancellationToken)
    {
        var funcionarios = await _context.Funcionarios
            .AsNoTracking()
            .Select(f => ToResponse(f))
            .ToListAsync(cancellationToken);

        return new ServiceResponse<List<FuncionarioResponse>>
        {
            Dados = funcionarios,
            Mensagem = funcionarios.Count == 0
                ? "Sem dados por enquanto!"
                : "Processo concluído com sucesso!"
        };
    }

    public async Task<ServiceResponse<FuncionarioResponse>> GetFuncionariosById(
        int id,
        CancellationToken cancellationToken)
    {
        var funcionario = await _context.Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                f => f.Id == id,
                cancellationToken);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        return new ServiceResponse<FuncionarioResponse>
        {
            Dados = ToResponse(funcionario),
            Mensagem = "Funcionário encontrado!"
        };
    }

    public async Task<ServiceResponse<FuncionarioResponse>> CreateFuncionarios(
        FuncionarioRequest request,
        CancellationToken cancellationToken)
    {
        var funcionario = new FuncionarioModel
        {
            Nome = request.Nome.Trim(),
            Sobrenome = request.Sobrenome.Trim(),
            Departamento = request.Departamento,
            Turno = request.Turno,
            Ativo = request.Ativo,
            DataDeCriacao = DateTime.UtcNow,
            DataDeAlteracao = DateTime.UtcNow
        };

        await _context.Funcionarios.AddAsync(
            funcionario,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<FuncionarioResponse>
        {
            Dados = ToResponse(funcionario),
            Mensagem = "Funcionário criado com sucesso!"
        };
    }

    public async Task<ServiceResponse<FuncionarioResponse>> UpdateFuncionarios(
        FuncionarioRequest request,
        int id,
        CancellationToken cancellationToken)
    {
        var funcionario = await _context.Funcionarios
            .FindAsync([id], cancellationToken);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        funcionario.Nome = request.Nome.Trim();
        funcionario.Sobrenome = request.Sobrenome.Trim();
        funcionario.Departamento = request.Departamento;
        funcionario.Turno = request.Turno;
        funcionario.Ativo = request.Ativo;
        funcionario.DataDeAlteracao = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<FuncionarioResponse>
        {
            Dados = ToResponse(funcionario),
            Mensagem = "Funcionário atualizado com sucesso!"
        };
    }

    public async Task<ServiceResponse<FuncionarioResponse>> InativaFuncionario(
        int id,
        CancellationToken cancellationToken)
    {
        var funcionario = await _context.Funcionarios
            .FindAsync([id], cancellationToken);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        funcionario.Ativo = false;
        funcionario.DataDeAlteracao = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<FuncionarioResponse>
        {
            Dados = ToResponse(funcionario),
            Mensagem = "Funcionário desativado com sucesso!"
        };
    }

    public async Task<ServiceResponse<bool>> DeleteFuncionarios(
        int id,
        CancellationToken cancellationToken)
    {
        var funcionario = await _context.Funcionarios
            .FindAsync([id], cancellationToken);

        if (funcionario is null)
        {
            return new ServiceResponse<bool>
            {
                Dados = false,
                Mensagem = "Funcionário não foi encontrado!",
                Sucesso = false
            };
        }

        _context.Funcionarios.Remove(funcionario);

        await _context.SaveChangesAsync(cancellationToken);

        return new ServiceResponse<bool>
        {
            Dados = true,
            Mensagem = "Funcionário deletado com sucesso!"
        };
    }

    private static FuncionarioResponse ToResponse(
        FuncionarioModel funcionario)
    {
        return new FuncionarioResponse
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Sobrenome = funcionario.Sobrenome,
            Departamento = funcionario.Departamento,
            Ativo = funcionario.Ativo,
            Turno = funcionario.Turno,
            DataDeCriacao = funcionario.DataDeCriacao,
            DataDeAlteracao = funcionario.DataDeAlteracao
        };
    }

    private static ServiceResponse<FuncionarioResponse>
        FuncionarioNaoEncontrado()
    {
        return new ServiceResponse<FuncionarioResponse>
        {
            Dados = null,
            Mensagem = "Funcionário não foi encontrado!",
            Sucesso = false
        };
    }
}