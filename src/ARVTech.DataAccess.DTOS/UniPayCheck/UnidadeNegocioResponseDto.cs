namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class UnidadeNegocioResponseDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public override string ToString()
        {
            return $"Unidade de Negócio ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}