namespace AcessePlus.Negocio.Excecao
{
    [Serializable]
    public class ExcecaoPersistencia : Exception
    {
        public ExcecaoPersistencia(string mensagem) : base(mensagem) { }
        public ExcecaoPersistencia(Exception innerException) : base(innerException.Message, innerException) { }
        public ExcecaoPersistencia(string mensagem, Exception innerException) : base(mensagem, innerException) { }
    }
}
