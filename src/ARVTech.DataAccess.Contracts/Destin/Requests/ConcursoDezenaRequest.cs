namespace ARVTech.DataAccess.Contracts.Destin.Requests
{
    using System;

    public class ConcursoDezenaRequest
    {
        public Guid? Id { get; set; }

        public required Guid IdConcurso { get; set; }

        public required short Dezena { get; set; }

        public override string ToString()
        {
            return $"Concurso Dezena Id: {this.Id}; Concurso Id: {this.IdConcurso}; Dezena: {this.Dezena}.";
        }
    }
}