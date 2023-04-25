namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CABANHAS")]
    public class CabanhaEntity
    {
        [Key]
        public Guid? Guid { get; set; }

        public byte[] Marca { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Cnpj { get; set; }

        public string Complemento { get; set; }

        public string Email { get; set; }

        public string Endereco { get; set; }

        [Column("NOME_FANTASIA")]
        public string NomeFantasia { get; set; }

        public string Numero { get; set; }

        [Column("PONTO_REFERENCIA")]
        public string PontoReferencia { get; set; }

        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; }

        public string Responsavel { get; set; }

        public string Telefone { get; set; }

        public string Uf { get; set; }

        public virtual AssociacaoEntity Associacao { get; set; }

        public virtual ContaEntity Conta { get; set; }
    }
}
