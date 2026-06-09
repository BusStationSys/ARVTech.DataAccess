namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    public record UnidadeNegocioResponse
    {
        public int Id { get; set; }

        public required string Descricao { get; set; }

        public override string ToString()
        {
            return $"Unidade de Negócio ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}