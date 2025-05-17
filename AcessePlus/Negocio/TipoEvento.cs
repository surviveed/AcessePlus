using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class TipoEvento
    {
        public void Salvar(Modelo.TipoEvento modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.TipoEvento().Atualizar(modelo);
            else
                new Persistencia.TipoEvento().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.TipoEvento().Excluir(Id);
        }
        public Modelo.TipoEvento BuscarPorId(int Id)
        {
            return new Persistencia.TipoEvento().BuscarPorId(Id);
        }
        public List<Modelo.TipoEvento> BuscarTodos()
        {
            return new Persistencia.TipoEvento().BuscarTodos();
        }
    }
}
