namespace ARVTech.DataAccess.Core.Entities.UniPayCheck
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PESSOAS")]
    public class PessoaEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("BAIRRO")]
        public virtual string Bairro { get; set; }

        [Description("CEP")]
        public virtual string Cep { get; set; }

        [Description("CIDADE")]
        public virtual string Cidade { get; set; }

        [Description("COMPLEMENTO")]
        public virtual string Complemento { get; set; }

        [Description("ENDERECO")]
        public virtual string Endereco { get; set; }

        [Description("NUMERO")]
        public virtual string Numero { get; set; }

        [Description("UF")]
        public virtual string Uf { get; set; }

        public override string ToString()
        {
            return $"Pessoa GUID: {this.Guid}.";
        }
    }
}