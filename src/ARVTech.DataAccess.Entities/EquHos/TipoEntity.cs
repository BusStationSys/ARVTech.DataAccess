namespace ARVTech.DataAccess.Entities.EquHos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TIPOS")]
    public class TipoEntity
    {
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        [Description("SEXO")]
        public virtual string Sexo { get; set; }

        [Description("OBSERVACOES")]
        public virtual string Observacoes { get; set; }

        [Description("ORDEM")]
        public virtual int Ordem { get; set; }

        [Description("COR")]
        public virtual string Cor { get; set; }

        [Description("ICONE")]
        public virtual string Icone { get; set; }

        [Description("EXIBIR_QUADRO_ANIMAIS")]
        public virtual bool ExibirQuadroAnimais { get; set; }

        public virtual ICollection<AnimalEntity> Animais { get; set; }

        public override string ToString()
        {
            return $"Tipo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}