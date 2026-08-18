namespace ARVTech.DataAccess.Contracts.PortalLoterias.Responses
{
    public class PremioRateioResponse
    {
        public string descricaoFaixa { get; set; }
        public int faixa { get; set; }
        public int numeroDeGanhadores { get; set; }
        public double valorPremio { get; set; }
    }
}