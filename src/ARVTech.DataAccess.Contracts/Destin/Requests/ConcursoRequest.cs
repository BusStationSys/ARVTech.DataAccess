namespace ARVTech.DataAccess.Contracts.Destin.Requests
{
    using System;
    using System.Collections.Generic;

    public class ConcursoRequest
    {
        public Guid? Id { get; set; }

        public required int IdModalidade { get; set; }

        public required int Numero { get; set; }

        public required DateTime DataApuracao { get; set; }

        public List<ConcursoDezenaRequest> Dezenas { get; set; }

        public override string ToString()
        {
            return $"Concurso IdModalidade: {this.IdModalidade}; Numero: {this.Numero}; DataApuracao: {this.DataApuracao}.";
        }
    }
}