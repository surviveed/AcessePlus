using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Local
    {
        public void Salvar(Modelo.Local modelo, List<IFormFile> imagens = null)
        {
            if (modelo.Id != 0)
            {
                new Persistencia.Local().Atualizar(modelo);
            }
            else
            {
                var persistenciaLocal = new Persistencia.Local();
                modelo.Id = persistenciaLocal.Inserir(modelo);
            }

            if (imagens != null && imagens.Count > 0)
            {
                var persistenciaImagem = new Persistencia.LocalImagem();

                for (int i = 0; i < imagens.Count; i++)
                {
                    var imagem = imagens[i];

                    using var ms = new MemoryStream();
                    imagem.CopyTo(ms);

                    var localImagem = new Modelo.LocalImagem
                    {
                        LocalId = modelo.Id,
                        Imagem = ms.ToArray(),
                        NomeArquivo = imagem.FileName,
                        Ordem = i,
                        DataCadastro = DateTime.Now
                    };

                    persistenciaImagem.Inserir(localImagem);
                }
            }
        }

        public void Excluir(int Id)
        {
            new Persistencia.Local().Excluir(Id);
        }
        public Modelo.Local BuscarPorId(int Id)
        {
            return new Persistencia.Local().BuscarPorId(Id);
        }
        public List<Modelo.Local> BuscarTodos()
        {
            return new Persistencia.Local().BuscarTodos();
        }
    }
}
