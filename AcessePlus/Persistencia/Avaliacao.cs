using Npgsql;
using Persistencia;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace AcessePlus.Persistencia
{
    public class Avaliacao: ConexaoBD
    {
        public string CamposTabela = "(comentario, tipo_acessibilidade, tipo, id_local)";
        public void Inserir(Modelo.Avaliacao modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("INSERT INTO avaliacao {0}" +
                " VALUES (@comentario,@tipo_acessibilidade,@tipo, @id_local);", CamposTabela));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("comentario", modelo.Comentario));
            comando.Parameters.Add(new NpgsqlParameter("tipo_acessibilidade", modelo.TipoAcessibilidade));
            comando.Parameters.Add(new NpgsqlParameter("tipo", modelo.Tipo));
            comando.Parameters.Add(new NpgsqlParameter("id_local", modelo.Local.Id));

            comando.ExecuteNonQuery();
        } 
        public void Excluir (int Id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("DELETE FROM avaliacao" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            comando.ExecuteNonQuery();
        }
        public void Atualizar(Modelo.Avaliacao modelo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("UPDATE avaliacao " +
                " SET comentario = @comentario, tipo_acessibilidade = @tipo_acessibilidade, tipo=@tipo, id_local=@id_local" +
                " WHERE id= @id"));

            NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), Conexao);

            comando.Parameters.Add(new NpgsqlParameter("comentario", modelo.Comentario));
            comando.Parameters.Add(new NpgsqlParameter("tipo_acessibilidade", modelo.TipoAcessibilidade));
            comando.Parameters.Add(new NpgsqlParameter("tipo", modelo.Tipo));
            comando.Parameters.Add(new NpgsqlParameter("id_local", modelo.Local.Id));
            comando.Parameters.Add(new NpgsqlParameter("id", modelo.Id));

            comando.ExecuteNonQuery();
        }
        public List<Modelo.Avaliacao> BuscarTodos()
        {
            List<Modelo.Avaliacao> modelos = new List<Modelo.Avaliacao>();

            var sql = "SELECT * FROM avaliacao;";

            NpgsqlCommand comando = new NpgsqlCommand(sql, Conexao);

            NpgsqlDataReader leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                var modelo = new Modelo.Avaliacao();

                modelo.Id = leitor.GetInt32(0);
                modelo.Comentario = leitor.GetString(1);
                modelo.TipoAcessibilidade_Enum = (Modelo.Avaliacao.eTipoAcessibilidade)leitor.GetChar(2);
                modelo.Tipo_Enum = (Modelo.Avaliacao.eTipo)leitor.GetChar(3);
                modelo.Local.Id = leitor.GetInt32(4);

                modelos.Add(modelo);
            }
            leitor.Close();

            //somente executar este loop se for preciso acessar a propriedade local em algum momento ao fazer a query

            //foreach (var modelo in modelos)
            //{
            //    var sqlLocal = "SELECT * FROM local WHERE id = @id;";
            //    NpgsqlCommand comandoLocal = new NpgsqlCommand(sqlLocal, Conexao);
            //    comandoLocal.Parameters.Add(new NpgsqlParameter("id", modelo.Local.Id));
            //    NpgsqlDataReader leitorLocal = comandoLocal.ExecuteReader();

            //    while (leitorLocal.Read())
            //    {
            //        var local = new Modelo.Local();
            //        local.Id = leitorLocal.GetInt32(0);
            //        local.Nome = leitorLocal.GetString(1);
            //        local.Capacidade = leitorLocal.GetInt32(2);
            //        local.Endereco = leitorLocal.GetString(3);
            //        local.Observacoes = leitorLocal.GetString(4);
            //        local.Cidade.Id = leitorLocal.GetInt32(5);
            //        modelo.Local = local;
            //    }
            //    leitorLocal.Close();
            //}

            return modelos;
        }
        public Modelo.Avaliacao BuscarPorId(int Id)
        {
            Modelo.Avaliacao modelo = null;

            List<Modelo.Avaliacao> modelos = new List<Modelo.Avaliacao>();

            var sql = "SELECT * FROM avaliacao WHERE id=@id;";

            NpgsqlCommand comando = new NpgsqlCommand(sql, Conexao);

            comando.Parameters.Add(new NpgsqlParameter("id", Id));

            NpgsqlDataReader leitor = comando.ExecuteReader();

            if (leitor.Read())
            {
                modelo = new Modelo.Avaliacao();

                modelo.Id = leitor.GetInt32(0);
                modelo.Comentario = leitor.GetString(1);
                modelo.TipoAcessibilidade_Enum = (Modelo.Avaliacao.eTipoAcessibilidade)leitor.GetChar(2);
                modelo.Tipo_Enum = (Modelo.Avaliacao.eTipo)leitor.GetChar(3);
                modelo.Local.Id = leitor.GetInt32(4);

                modelos.Add(modelo);
            }
            leitor.Close();

            return modelo;
        }
    }
}
