namespace ARVTech.DataAccess.Contracts.PayCheck.Requests
{
    public class CalculoRequest
    {
        public int? Id { get; set; }

        public string Descricao { get; set; }

        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Cálculo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}