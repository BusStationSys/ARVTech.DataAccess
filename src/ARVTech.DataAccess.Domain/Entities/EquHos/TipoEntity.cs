namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TIPOS")]
    public class TipoEntity
    {
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public int Id { get; set; }

        [Description("DESCRICAO")]
        public string Descricao { get; set; }

        [Description("SEXO")]
        public string Sexo { get; set; }

        [Description("OBSERVACOES")]
        public string Observacoes { get; set; }

        [Description("ORDEM")]
        public int Ordem { get; set; }

        [Description("COR")]
        public string Cor { get; set; }

        [Description("ICONE")]
        public string Icone { get; set; }

        [Description("EXIBIR_QUADRO_ANIMAIS")]
        public bool ExibirQuadroAnimais { get; set; }

        public ICollection<AnimalEntity> Animais { get; set; }

        public override string ToString()
        {
            return $"Tipo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}