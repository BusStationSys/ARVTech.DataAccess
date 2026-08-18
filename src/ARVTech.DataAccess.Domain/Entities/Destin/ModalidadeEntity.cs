namespace ARVTech.DataAccess.Domain.Entities.Destin
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Modalidade")]
    public class ModalidadeEntity
    {
        [Description("Id")]
        public int Id { get; set; }

        [Description("Descricao")]
        public string Descricao { get; set; }

        public int? UltimoConcursoApurado { get; set; }

        public override string ToString()
        {
            return $"Modalidade Id: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}