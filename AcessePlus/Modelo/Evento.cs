namespace AcessePlus.Modelo
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Local Local { get; set; }
        public TipoEvento TipoEvento {get;set;} 
    }
}
