namespace AcessePlus.Modelo
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int CodigoIbge { get; set; }
        public Uf Uf { get; set; }
        public Cidade()
        {
            Uf = new Uf();
        }
    }
}
