namespace AcessePlus.Modelo
{
    public class Local
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public TipoLocal TipoLocal { get; set; }
        public string Endereco { get; set; }
        public Cidade Cidade { get; set; }
        public Local()
        {
            Cidade = new Cidade();
        }
        public string ImagemUrl { get; set; }
    }
}
