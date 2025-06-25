namespace AcessePlus.Models.ViewModels
{
    public class LocalViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int Capacidade { get; set; }
        public double? Rating { get; set; }
        public string ImagemUrl { get; set; }
    }
}
