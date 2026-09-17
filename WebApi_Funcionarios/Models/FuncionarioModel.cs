using System.ComponentModel.DataAnnotations;
using WebApi_ASPNETCore.Enums;

namespace WebApi_ASPNETCore.Models;

public class FuncionarioModel
{
    [Key]
    public int Id { get; set; }

    public required string Nome { get; set; }

    public required string Sobrenome { get; set; }

    public DepartamentoEnum Departamento { get; set; }

    public bool Ativo { get; set; } = true;

    public TurnoEnum Turno { get; set; }

    public DateTime DataDeCriacao { get; set; }

    public DateTime DataDeAlteracao { get; set; }
}
