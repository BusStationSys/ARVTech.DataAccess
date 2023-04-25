namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;
    using System.Collections.Generic;

    public class AssociacaoDto
    {
        public int? Id { get; set; }

        public string RazaoSocial { get; set; }

        public string Sigla { get; set; }

        public string Observacoes { get; set; }

        public string DescricaoRegistro { get; set; }

        public ICollection<CabanhaDto> Cabanhas { get; set; }

        public override string ToString()
        {
            return $"Associação ID: {this.Id}; Razão Social: {this.RazaoSocial}.";
        }
    }
}