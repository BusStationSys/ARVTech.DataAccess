namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UNIDADES_NEGOCIO")]
    public class UnidadeNegocioEntity
    {
        [Description("ID")]
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        public override string ToString()
        {
            return $"Unidade de Negócio ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}