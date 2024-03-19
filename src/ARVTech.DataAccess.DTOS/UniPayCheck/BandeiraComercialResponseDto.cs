namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class BandeiraComercialResponseDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public override string ToString()
        {
            return $"Bandeira Comercial ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}