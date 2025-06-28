using Npgsql;

namespace Persistencia
{
    public abstract class ConexaoBD
    {
        private NpgsqlConnection _conexaoPostgres;

        public NpgsqlConnection Conexao
        {
            get
            {
                if (_conexaoPostgres == null)
                {
                    string con = "";

                    if (Environment.UserName == "Gabriela - PLMPRO" )
                    {
                        con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=postgres;Password=ucs";
                    }
                    else if(Environment.UserName == "Francisco")
                    {
                        con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=postgres;Password=ucs";
                    }
                    else
                    {
                        con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=leonardobitencourt;Password=1234";
                    }

                    _conexaoPostgres = new NpgsqlConnection(con);
                    _conexaoPostgres.Open();
                }

                return _conexaoPostgres;
            }
        }
        public static NpgsqlConnection GetConnection()
        {
            string con;

            if (Environment.UserName == "Gabriela - PLMPRO")
                con = "Server=localhost;Port=5432;Database=acesseplus;User ID=postgres;Password=ucs";
            else if (Environment.UserName == "Francisco")
                con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=postgres;Password=ucs";
            else
                con = "Server=localhost;Port=5432;Database=AcessePlus;User ID=leonardobitencourt;Password=1234";

            var conexao = new NpgsqlConnection(con);
            conexao.Open();
            return conexao;
        }
    }
}
