using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AcessePlus.Modelo
{
    public class Cidade
    {
        public Cidade()
        {
            Uf = new Uf();
        }

        public int Id { get; set; }

        [ValidateNever]
        public string Descricao { get; set; }

        [ValidateNever]
        public Uf Uf { get; set; }

        [ValidateNever]
        public int CodigoIbge { get; set; }
    }
}
