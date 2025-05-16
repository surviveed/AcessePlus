using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Pais
    {
        public void Salvar(Modelo.Pais modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Pais().Atualizar(modelo);
            else
                new Persistencia.Pais().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.Pais().Excluir(Id);
        }
        public Modelo.Pais BuscarPorId(int Id)
        {
            return new Persistencia.Pais().BuscarPorId(Id);
        }
        public List<Modelo.Pais> BuscarTodos()
        {
            return new Persistencia.Pais().BuscarTodos();
        }
    }
}
