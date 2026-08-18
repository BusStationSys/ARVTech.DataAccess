namespace ARVTech.DataAccess.Domain.Entities.Destin
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ConcursoDezena")]
    public class ConcursoDezenaEntity
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