namespace AcessePlus.Modelo
{
    public class LocalImagem
    {
        public int Id { get; set; }
        public int LocalId { get; set; }
        public byte[] Imagem { get; set; }
        public string NomeArquivo { get; set; }
        public int Ordem { get; set; }
        public DateTime DataCadastro { get; set; }
        public Local Local { get; set; }
    }
}
