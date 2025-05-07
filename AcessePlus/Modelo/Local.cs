namespace AcessePlus.Modelo
{
    public class Local
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public string Endereco { get; set; }
        public string Observacoes { get; set; }
        public Cidade Cidade { get; set; }
        public Local()
        {
            Cidade = new Cidade();
        }
    }
}
