using WebApi_ASPNETCore.Enums;

namespace WebApi_ASPNETCore.DTOs.Funcionario;

public class FuncionarioResponse
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Sobrenome { get; set; }
    public DepartamentoEnum Departamento { get; set; }
    public bool Ativo { get; set; }
    public TurnoEnum Turno { get; set; }
    public DateTime DataDeCriacao { get; set; }
    public DateTime DataDeAlteracao { get; set; }
}
