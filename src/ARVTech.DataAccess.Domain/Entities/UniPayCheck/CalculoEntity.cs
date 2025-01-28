namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CALCULOS")]
    public class CalculoEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Cálculo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}