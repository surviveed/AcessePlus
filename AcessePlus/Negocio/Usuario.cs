namespace AcessePlus.Negocio
{
    public class Usuario
    {
        public Modelo.Usuario SearchEmailAndPassword(string email, string senha)
        {
            return new Persistencia.Usuario().BuscarPorEmailSenha(email, senha);
        }

        public void Cadastrar(Modelo.Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome) ||
                string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.Senha))
            {
                throw new Exception("Preencha todos os campos obrigatórios.");
            }

            new Persistencia.Usuario().Inserir(usuario);
        }
    }
}
