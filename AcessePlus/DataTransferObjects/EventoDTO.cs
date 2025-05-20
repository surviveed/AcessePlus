namespace AcessePlus.DataTransferObjects
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public LocalDTO Local { get; set; }
        public TipoEventoDTO TipoEvento { get; set; }
    }
}

