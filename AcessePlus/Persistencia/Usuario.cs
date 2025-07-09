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
                    Senha = reader["Senha"].ToString(),
                    Nivel = Convert.ToInt32(reader["Nivel"])
                };
            }

            return null;
        }

        public void Inserir(Modelo.Usuario usuario)
        {
            using var con = ConexaoBD.GetConnection();
            var cmd = new NpgsqlCommand(
                "INSERT INTO Usuario (Nome, Email, Senha, Nivel) VALUES (@Nome, @Email, @Senha, @Nivel)", con);

            cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
            cmd.Parameters.AddWithValue("@Nivel", (int)usuario.Nivel);

            cmd.ExecuteNonQuery();
        }
    }
}
