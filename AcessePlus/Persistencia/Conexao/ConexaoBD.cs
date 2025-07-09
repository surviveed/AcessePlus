using Npgsql;

namespace Persistencia
{
    public abstract class ConexaoBD
    {
        public static NpgsqlConnection GetConnection()
        {
            string con;

            if (Environment.UserName == "Gabriela - PLMPRO" || Environment.UserName == "Francisco")
            {
                con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=postgres;Password=ucs";
            }
            else
            {
                con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=leonardobitencourt;Password=1234";
            }

            var conexao = new NpgsqlConnection(con);
            conexao.Open();
            return conexao;
        }
    }
}
