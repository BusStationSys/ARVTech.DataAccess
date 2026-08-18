namespace ARVTech.DataAccess.Contracts.Destin.Responses
{
    using System;

    public record ConcursoDezenaResponse
    {
        public Guid Id { get; set; }

        public Guid IdConcurso { get; set; }

        public short Dezena { get; set; }

        public override string ToString()
        {
            return $"Concurso Dezena Id: {this.Id}; Concurso Id: {this.IdConcurso}; Dezena: {this.Dezena}.";
        }
    }
}