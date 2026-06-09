namespace ARVTech.DataAccess.Contracts.PayCheck.Requests
{
    public class EventoRequest
    {
        public int? Id { get; set; }

        public required string Descricao { get; set; }

        public required string Tipo { get; set; }

        public string? Observacoes { get; set; }

        public override string ToString()
        {
            return $"Evento ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}