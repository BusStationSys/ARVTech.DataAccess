namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TOTALIZADORES")]
    public class TotalizadorEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Totalizador ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}