using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Models;

namespace WebApi_ASPNETCore.Service.FuncionarioService;

public interface IFuncionarioInterface
{
    Task<ServiceResponse<List<FuncionarioResponse>>> GetFuncionarios(
        CancellationToken cancellationToken);

    Task<ServiceResponse<FuncionarioResponse>> GetFuncionariosById(
        int id,
        CancellationToken cancellationToken);

    Task<ServiceResponse<FuncionarioResponse>> CreateFuncionarios(
        FuncionarioRequest request,
        CancellationToken cancellationToken);

    Task<ServiceResponse<FuncionarioResponse>> UpdateFuncionarios(
        FuncionarioRequest request,
        int id,
        CancellationToken cancellationToken);

    Task<ServiceResponse<FuncionarioResponse>> InativaFuncionario(
        int id,
        CancellationToken cancellationToken);

    Task<ServiceResponse<bool>> DeleteFuncionarios(
        int id,
        CancellationToken cancellationToken);
}