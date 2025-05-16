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
    }
}
