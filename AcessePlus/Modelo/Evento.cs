namespace AcessePlus.Modelo
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Local Local { get; set; }
        public TipoEvento TipoEvento {get;set;} 

        public Evento()
        {
            Local = new Local();
            TipoEvento = new TipoEvento();
        }
    }
}
