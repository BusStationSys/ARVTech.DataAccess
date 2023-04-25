namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;
    using System.Collections.Generic;

    public class PelagemDto
    {
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public string Observacoes { get; set; }

        public ICollection<AnimalDto> Animais { get; set; }
    }
}