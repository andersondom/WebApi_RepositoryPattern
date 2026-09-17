using Microsoft.EntityFrameworkCore;
using WebApi_ASPNETCore.DataContext;
using WebApi_ASPNETCore.Models;

namespace WebApi_ASPNETCore.Service.FuncionarioService;

public class FuncionarioService : IFuncionarioInterface
{
    private readonly ApplicationDbContext _context;

    public FuncionarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<FuncionarioModel>>> GetFuncionarios()
    {
        var funcionarios = await _context.Funcionarios
            .AsNoTracking()
            .ToListAsync();

        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = funcionarios,
            Mensagem = funcionarios.Count == 0
                ? "Sem dados por enquanto!"
                : "Processo concluído com sucesso!"
        };
    }

    public async Task<ServiceResponse<FuncionarioModel>> GetFuncionariosById(int id)
    {
        var funcionario = await _context.Funcionarios
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

        if (funcionario is null)
        {
            return new ServiceResponse<FuncionarioModel>
            {
                Dados = null,
                Mensagem = "Funcionário não foi encontrado!",
                Sucesso = false
            };
        }

        return new ServiceResponse<FuncionarioModel>
        {
            Dados = funcionario,
            Mensagem = "Funcionário encontrado!"
        };
    }

    public async Task<ServiceResponse<List<FuncionarioModel>>> CreateFuncionarios(
        FuncionarioModel modelCreate)
    {
        await _context.Funcionarios.AddAsync(modelCreate);
        await _context.SaveChangesAsync();

        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = await GetFuncionariosList(),
            Mensagem = "Funcionário criado com sucesso!"
        };
    }

    public async Task<ServiceResponse<List<FuncionarioModel>>> UpdateFuncionarios(
        FuncionarioModel modelUpdate,
        int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        funcionario.Nome = modelUpdate.Nome;
        funcionario.Sobrenome = modelUpdate.Sobrenome;
        funcionario.Departamento = modelUpdate.Departamento;
        funcionario.Ativo = modelUpdate.Ativo;
        funcionario.Turno = modelUpdate.Turno;
        funcionario.DataDeAlteracao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = await GetFuncionariosList(),
            Mensagem = "Funcionário atualizado com sucesso!"
        };
    }

    public async Task<ServiceResponse<List<FuncionarioModel>>> InativaFuncionario(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        funcionario.Ativo = false;
        funcionario.DataDeAlteracao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = await GetFuncionariosList(),
            Mensagem = "Funcionário desativado com sucesso!"
        };
    }

    public async Task<ServiceResponse<List<FuncionarioModel>>> DeleteFuncionarios(int id)
    {
        var funcionario = await _context.Funcionarios.FindAsync(id);

        if (funcionario is null)
            return FuncionarioNaoEncontrado();

        _context.Funcionarios.Remove(funcionario);
        await _context.SaveChangesAsync();

        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = await GetFuncionariosList(),
            Mensagem = "Funcionário deletado com sucesso!"
        };
    }

    private async Task<List<FuncionarioModel>> GetFuncionariosList()
    {
        return await _context.Funcionarios
            .AsNoTracking()
            .ToListAsync();
    }

    private static ServiceResponse<List<FuncionarioModel>> FuncionarioNaoEncontrado()
    {
        return new ServiceResponse<List<FuncionarioModel>>
        {
            Dados = null,
            Mensagem = "Funcionário não foi encontrado!",
            Sucesso = false
        };
    }
}