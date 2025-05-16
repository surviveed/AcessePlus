using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Uf
    {
        public void Salvar(Modelo.Uf modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Uf().Atualizar(modelo);
            else
                new Persistencia.Uf().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.Uf().Excluir(Id);
        }
        public Modelo.Uf BuscarPorId(int Id)
        {
            return new Persistencia.Uf().BuscarPorId(Id);
        }
        public List<Modelo.Uf> BuscarTodos()
        {
            return new Persistencia.Uf().BuscarTodos();
        }
    }
}
