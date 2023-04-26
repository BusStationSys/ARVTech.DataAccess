namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;
    using System.Collections.Generic;

    public class TipoDto
    {
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public string Sexo { get; set; }

        public string Observacoes { get; set; }

        public int Ordem { get; set; }

        public string Cor { get; set; }

        public string Icone { get; set; }

        public bool ExibirQuadroAnimais { get; set; }

        public ICollection<AnimalDto> Animais { get; set; }

        public override string ToString()
        {
            return $"Tipo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}