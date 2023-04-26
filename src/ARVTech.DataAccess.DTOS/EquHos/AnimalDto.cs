namespace ARVTech.DataAccess.DTOs.EquHos
{
    using System;

    public class AnimalDto
    {
        public Guid? Guid { get; set; }

        public string Nome { get; set; }

        public PelagemDto Pelagem { get; set; }

        public override string ToString()
        {
            return $"Animal ID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}