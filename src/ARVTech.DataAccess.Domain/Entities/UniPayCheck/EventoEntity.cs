namespace ARVTech.DataAccess.Domain.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EVENTOS")]
    public class EventoEntity
    {
        [Description("ID")]
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        [Description("TIPO")]
        public string Tipo { get; set; }

        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Evento ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}