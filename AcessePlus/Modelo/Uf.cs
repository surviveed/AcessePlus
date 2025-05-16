namespace AcessePlus.Modelo
{
    public class Uf
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int CodigoIbge { get; set; }
        public Pais Pais { get; set; }
        public Uf()
        {
            Pais = new Pais();
        }
    }
}
