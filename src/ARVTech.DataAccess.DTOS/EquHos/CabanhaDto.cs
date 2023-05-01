namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;

    public class CabanhaDto
    {
        public Guid? Guid { get; set; }

        public byte[] Marca { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Cnpj { get; set; }

        public string Complemento { get; set; }

        public string Email { get; set; }

        public string Endereco { get; set; }

        public string NomeFantasia { get; set; }

        public string Numero { get; set; }

        public string PontoReferencia { get; set; }

        public string RazaoSocial { get; set; }

        public string Responsavel { get; set; }

        public string Telefone { get; set; }

        public string Uf { get; set; }

        public int? IdAssociacao { get; set; }

        public AssociacaoDto Associacao { get; set; }

        public Guid? GuidConta { get; set; }

        public ContaDto Conta { get; set; }

        public override string ToString()
        {
            return $"Cabanha GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}