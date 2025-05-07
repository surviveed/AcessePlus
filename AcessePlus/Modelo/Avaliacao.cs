using System.ComponentModel;

namespace AcessePlus.Modelo
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public string Comentario { get; set;}
        public enum eTipoAcessibilidade
        {
            [Description("Auditiva")]
            Auditiva = 'A',

            [Description("Visual")]
            Visual = 'V',

            [Description("Locomotiva")]
            Locomotiva = 'L'
        }
        public eTipoAcessibilidade TipoAcessibilidade_Enum { get; set; }
        public char TipoAcessibilidade { get { return Convert.ToChar(TipoAcessibilidade_Enum); } }
        public enum eTipo
        {
            [Description("Positiva")]
            Positiva = 'P',

            [Description("Negativa")]
            Negativa = 'N'
        }
        public eTipo Tipo_Enum { get; set; }
        public char Tipo { get { return Convert.ToChar(Tipo_Enum); } }
        public Local Local { get; set; }

        public Avaliacao()
        {
            Local = new Local();
        }
    }
}
