namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CALCULOS")]
    public class CalculoEntity
    {
        [Description("ID")]
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        [Description("OBSERVACOES")]
        public virtual string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Cáluclo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}