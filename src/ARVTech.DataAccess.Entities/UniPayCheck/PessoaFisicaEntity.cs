namespace ARVTech.DataAccess.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PESSOAS_FISICAS")]
    public class PessoaFisicaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("GUIDPESSOA")]
        public virtual Guid GuidPessoa { get; set; }

        public virtual PessoaEntity Pessoa { get; set; }

        [Description("CPF")]
        public virtual string CPF { get; set; }

        [Description("RG")]
        public virtual string RG { get; set; }

        [Description("DATA_NASCIMENTO")]
        public virtual DateTime? DATA_NASCIMENTO { get; set; }

        [Description("NOME")]
        public virtual string NOME { get; set; }

        public override string ToString()
        {
            return $"Pessoa Física GUID: {this.Guid}; Pessoa {this.GuidPessoa}.";
        }
    }
}