namespace ARVTech.DataAccess.Contracts.Destin.Responses
{
    using System;

    public record ModalidadeResponse
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int? UltimoConcursoApurado { get; set; }

        public override string ToString()
        {
            return $"Modalidade Id: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}