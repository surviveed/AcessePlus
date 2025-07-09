using AcessePlus.Modelo;

public class LocalCreateViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Capacidade { get; set; }
    public string Endereco { get; set; }
    public int? PaisId { get; set; }
    public int TipoLocalId { get; set; }
    public int? UfId { get; set; }
    public int? CidadeId { get; set; }
    public List<IFormFile> Imagens { get; set; } = new();
}