using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Evento
    {
        public void Salvar(Modelo.Evento modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Evento().Atualizar(modelo);
            else
                new Persistencia.Evento().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.Evento().Excluir(Id);
        }
        public Modelo.Evento BuscarPorId(int Id)
        {
            return new Persistencia.Evento().BuscarPorId(Id);
        }
        public List<Modelo.Evento> BuscarTodos()
        {
            var modelos=  new Persistencia.Evento().BuscarTodos();

            foreach(var modelo in modelos)
            {
                modelo.Local = new Negocio.Local().BuscarPorId(modelo.Local.Id);
                modelo.TipoEvento = new Negocio.TipoEvento().BuscarPorId(modelo.TipoEvento.Id);
            }

            return modelos; 
        }
    }
}
