namespace ARVTech.DataAccess.Contracts.Destin.Requests.Create
{
    public class ModalidadeCreateRequest
    {
        public string Descricao { get; set; }

        public override string ToString()
        {
            return $"Descrição: {this.Descricao}.";
        }
    }
}