using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Cidade
    {
        public void Salvar(Modelo.Cidade modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Cidade().Atualizar(modelo);
            else
                new Persistencia.Cidade().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.Cidade().Excluir(Id);
        }
        public Modelo.Cidade BuscarPorId(int Id)
        {
            return new Persistencia.Cidade().BuscarPorId(Id);
        }
        public List<Modelo.Cidade> BuscarTodos()
        {
            return new Persistencia.Cidade().BuscarTodos();
        }
    }
}
