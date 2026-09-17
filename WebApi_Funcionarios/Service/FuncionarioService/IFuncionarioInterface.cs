using WebApi_ASPNETCore.DTOs.Funcionario;
using WebApi_ASPNETCore.Models;

namespace WebApi_ASPNETCore.Service.FuncionarioService;

public interface IFuncionarioInterface
{
    Task<ServiceResponse<List<FuncionarioResponse>>> GetFuncionarios();

    Task<ServiceResponse<FuncionarioResponse>> GetFuncionariosById(int id);

    Task<ServiceResponse<FuncionarioResponse>> CreateFuncionarios(
        FuncionarioRequest request);

    Task<ServiceResponse<FuncionarioResponse>> UpdateFuncionarios(
        FuncionarioRequest request,
        int id);

    Task<ServiceResponse<FuncionarioResponse>> InativaFuncionario(int id);

    Task<ServiceResponse<bool>> DeleteFuncionarios(int id);
}