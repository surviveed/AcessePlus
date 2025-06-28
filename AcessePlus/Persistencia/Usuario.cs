using AcessePlus.Modelo;
using Npgsql;
using Persistencia;

namespace AcessePlus.Persistencia
{
    public class Usuario
    {
        public Modelo.Usuario BuscarPorEmailSenha(string email, string senha)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand("SELECT * FROM Usuario WHERE Email = @Email AND Senha = @Senha", con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Modelo.Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString(),
                    Email = reader["Email"].ToString(),
                    Senha = reader["Senha"].ToString()
                };
            }

            return null;
        }

        public void Inserir(Modelo.Usuario usuario)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand("INSERT INTO Usuario (Nome, Email, Senha) VALUES (@Nome, @Email, @Senha)", con);
            cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Senha", usuario.Senha);

            cmd.ExecuteNonQuery();
        }
    }
}
