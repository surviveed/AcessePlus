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
                    _conexaoPostgres = new NpgsqlConnection("Server=localhost;Port=5432;Database=AcessePlus;User ID=leonardobitencourt;Password=1234");
                    _conexaoPostgres.Open();
                }

                return _conexaoPostgres;
            }
        }

    }
}
