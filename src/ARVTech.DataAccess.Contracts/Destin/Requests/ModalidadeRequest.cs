using System;

namespace ARVTech.DataAccess.Contracts.Destin.Requests
{
    public class ModalidadeRequest
    {
        public int? Id { get; set; }

        public required string Descricao { get; set; }

        public int? UltimoConcursoApurado { get; set; }

        public override string ToString()
        {
            if (this.Id.HasValue)
                return $"Modalidade Id: {this.Id}; Descrição: {this.Descricao}.";
            else
                return $"Modalidade Descrição: {this.Descricao}.";
        }
    }
}