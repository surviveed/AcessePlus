using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AcessePlus.Modelo
{
    public class Uf
    {
        public int Id { get; set; }

        [ValidateNever]
        public string Descricao { get; set; }

        [ValidateNever]
        public Pais Pais { get; set; }

        [ValidateNever]
        public int CodigoIbge { get; set; }
    }
}
