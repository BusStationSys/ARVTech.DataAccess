namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EVENTOS")]
    public class EventoEntity
    {
        [Description("ID")]
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        [Description("TIPO")]
        public virtual string Tipo { get; set; }

        [Description("OBSERVACOES")]
        public virtual string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Evento ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}