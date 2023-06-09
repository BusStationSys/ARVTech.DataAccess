namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    public class EventoDto
    {
        public int? Id { get; set; }

        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Tipo { get; set; }

        public string Observacoes { get; set; }

        public override string ToString()
        {
            return $"Evento ID: {this.Id}; Código: {this.Codigo}; Descrição: {this.Descricao}.";
        }
    }
}