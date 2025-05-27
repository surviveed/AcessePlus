namespace AcessePlus.DataTransferObjects
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public int LocalId { get; set; }
        public int TipoEventoId { get; set; }
    }
}
