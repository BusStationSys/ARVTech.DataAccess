namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;

    public class CabanhaDto
    {
        public Guid? Guid { get; set; }

        public string Cnpj { get; set; }

        public string RazaoSocial { get; set; }

        public AssociacaoDto Associacao { get; set; }

        public override string ToString()
        {
            return $"Cabanha GUID: {this.Guid}; Razão Social: {this.RazaoSocial}.";
        }
    }
}