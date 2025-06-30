using System.Collections.Generic;

namespace AcessePlus.Negocio
{
    public class TipoLocal
    {
        public void Salvar(Modelo.TipoLocal modelo)
        {
            if (modelo.Id != 0)
            {
                new Persistencia.TipoLocal().Atualizar(modelo);
            }
            else
            {
                var persistenciaTipoLocal = new Persistencia.TipoLocal();
                modelo.Id = persistenciaTipoLocal.Inserir(modelo);
            }
        }

        public void Excluir(int Id)
        {
            new Persistencia.TipoLocal().Excluir(Id);
        }

        public Modelo.TipoLocal BuscarPorId(int Id)
        {
            return new Persistencia.TipoLocal().BuscarPorId(Id);
        }

        public List<Modelo.TipoLocal> BuscarTodos()
        {
            return new Persistencia.TipoLocal().BuscarTodos();
        }
    }
}
