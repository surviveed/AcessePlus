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
                    _conexaoPostgres = new NpgsqlConnection("Server=localhost;Port=5432;Database=AcessePlus;User ID=postgres;Password=ucs");
                    _conexaoPostgres.Open();
                }

                return _conexaoPostgres;
            }
        }

    }
}
