namespace ARVTech.DataAccess.Domain.Entities.Destin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Concurso")]
    public class ConcursoEntity
    {
        public Guid Id { get; set; }

        public int IdModalidade { get; set; }

        public int Numero { get; set; }

        public DateTime DataApuracao { get; set; }

        public List<ConcursoDezenaEntity> Dezenas { get; set; }

        public override string ToString()
        {
            return $"Concurso Id: {this.Id}; IdModalidade: {this.IdModalidade}; Numero: {this.Numero}; DataApuracao: {this.DataApuracao}.";
        }
    }
}