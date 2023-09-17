namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class CalculoResponseDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Cálculo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}