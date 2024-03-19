namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BANDEIRAS_COMERCIAIS")]
    public class BandeiraComercialEntity
    {
        [Description("ID")]
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        public override string ToString()
        {
            return $"Bandeira Comercial ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}