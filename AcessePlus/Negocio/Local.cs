using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Local
    {
        public void Salvar(Modelo.Local modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Local().Atualizar(modelo);
            else
                new Persistencia.Local().Inserir(modelo);
        }
        public void Excluir(int Id)
        {
            new Persistencia.Local().Excluir(Id);
        }
    }
}
