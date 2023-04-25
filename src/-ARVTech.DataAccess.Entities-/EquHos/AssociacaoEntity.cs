using System.ComponentModel.DataAnnotations.Schema;

namespace ARVTech.DataAccess.Entities.EquHos
{
    [Table("ASSOCIACOES")]
    public class AssociacaoEntity
    {
        [Key]
        public int? Id { get; set; }

        [Column("DESCRICAO_REGISTRO")]
        public string DescricaoRegistro { get; set; }

        public string Observacoes { get; set; }

        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        public string Sigla { get; set; }
    }
}

/*

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Icms")]
    public class IcmsEntity
    {
        [Key]
        [Description("Código do ICMS")] //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public short CodigoIcms { get; set; }

        [Description("Descrição da Tributação")]
        public string DescricaoTributacao { get; set; }

        [Description("Alíquota")]
        public double Aliquota { get; set; }

        [Description("% Redução")]
        public double PercentualReducao { get; set; }

        public string Indicadores { get; set; }

        [Description("Data da Última Alteração")]
        public DateTime DataUltimaAlteracao { get; set; }

        [Description("Observação na NFS")]
        public string ObservacaoNFS { get; set; }

        [Description("% Alíquota FCP")]
        public double PercentualAliquotaFCP { get; set; }

        [Description("% Diferimento")]
        public double PercentualDiferimento { get; set; }

        public IEnumerable<ValidadeIcmsEntity> ValidadeIcms { get; set; }

        public override string ToString()
        {
            return $"Código ICMS: {this.CodigoIcms}; Descrição na Tributação: {this.DescricaoTributacao}; Alíquota: {this.Aliquota:0.00}; Percentual de Redução: {this.PercentualReducao:0.0000}.";
        }
    }
} 

 */