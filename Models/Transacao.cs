namespace FinancasAPI.Models;

public class Transacao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = "";
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "";
    public string Categoria { get; set; } = "";
    public DateTime DataMovimentacao { get; set; }
}
