namespace AcessePlus.Negocio.Excecao
{
    [Serializable]
    public class ExcecaoNegocio : Exception
    {
        public ExcecaoNegocio(string mensagem) : base(mensagem) { }
        public ExcecaoNegocio(Exception innerException) : base(innerException.Message, innerException) { }
        public ExcecaoNegocio(string mensagem, Exception innerException) : base(mensagem, innerException) { }
    }
}
