namespace ARVTech.DataAccess.Contracts.Destin.Responses
{
    using System;
    using System.Collections.Generic;

    public record ConcursoResponse
    {
        public Guid Id { get; set; }

        public int IdModalidade { get; set; }

        public int Numero { get; set; }

        public DateTime DataApuracao { get; set; }

        public IReadOnlyList<ConcursoDezenaResponse> Dezenas { get; set; }

        public override string ToString()
        {
            return $"Concurso Id: {this.Id}; IdModalidade: {this.IdModalidade}; Numero: {this.Numero}; DataApuracao: {this.DataApuracao}.";
        }
    }
}