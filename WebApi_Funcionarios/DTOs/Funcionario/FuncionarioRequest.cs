using System.ComponentModel.DataAnnotations;
using WebApi_ASPNETCore.Enums;

namespace WebApi_ASPNETCore.DTOs.Funcionario;

public class FuncionarioRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "O sobrenome é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public required string Sobrenome { get; set; }

    [EnumDataType(typeof(DepartamentoEnum))]
    public DepartamentoEnum Departamento { get; set; }

    [EnumDataType(typeof(TurnoEnum))]
    public TurnoEnum Turno { get; set; }

    public bool Ativo { get; set; } = true;
}
