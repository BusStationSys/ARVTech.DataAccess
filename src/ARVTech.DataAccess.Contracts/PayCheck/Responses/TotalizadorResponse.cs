namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    public record TotalizadorResponse
    {
        public int Id { get; set; }

        public required string Descricao { get; set; }

        public string? Observacoes { get; set; }

        public override string ToString()
        {
            return $"Totalizador ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}