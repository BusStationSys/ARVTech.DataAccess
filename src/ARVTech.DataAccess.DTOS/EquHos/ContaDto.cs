namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;
    using System.Collections.Generic;

    public class ContaDto
    {
        public Guid? Guid { get; set; }

        public string Cnpj { get; set; }

        public string RazaoSocial { get; set; }

        public string NomeFantasia { get; set; }

        public ICollection<CabanhaDto> Cabanhas { get; set; }

        public override string ToString()
        {
            return $"Conta GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}