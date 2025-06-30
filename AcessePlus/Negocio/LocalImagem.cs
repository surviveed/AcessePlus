using System.Collections.Generic;
using System.Linq;

namespace AcessePlus.Negocio
{
    public class LocalImagem
    {
        private readonly Persistencia.LocalImagem persistencia = new();

        public void Salvar(Modelo.LocalImagem imagem)
        {
            persistencia.Inserir(imagem);
        }

        public List<Modelo.LocalImagem> BuscarTodos()
        {
            return persistencia.BuscarTodos();
        }

        public List<Modelo.LocalImagem> BuscarPorLocal(int localId)
        {
            return persistencia.BuscarTodos()
                .Where(imagem => imagem.LocalId == localId)
                .ToList();
        }
    }
}
