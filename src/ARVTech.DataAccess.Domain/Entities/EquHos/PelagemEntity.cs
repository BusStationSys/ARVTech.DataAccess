namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PELAGENS")]
    public class PelagemEntity
    {
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        public ICollection<AnimalEntity> Animais { get; set; }

        public override string ToString()
        {
            return $"Pelagem ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}