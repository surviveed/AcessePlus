using System.ComponentModel;

namespace AcessePlus.Negocio
{
    public class Avaliacao
    {
        public void Salvar(Modelo.Avaliacao modelo)
        {
            if (modelo.Id != 0)
                new Persistencia.Avaliacao().Atualizar(modelo);
            else
                new Persistencia.Avaliacao().Inserir(modelo);
        } 
        public void Excluir (int Id)
        {
            new Persistencia.Avaliacao().Excluir(Id);
        }
    }
}
