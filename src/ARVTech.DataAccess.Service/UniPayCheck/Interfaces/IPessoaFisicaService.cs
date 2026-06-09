namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public interface IPessoaFisicaService
    {
        void Delete(Guid guid);

        PessoaFisicaResponse? Get(Guid guid);

        IEnumerable<PessoaFisicaResponse> GetAll();

        IEnumerable<PessoaFisicaResponse> GetAniversariantes(string periodoInicialString, string periodoFinalString);

        PessoaFisicaResponse GetByCpf(string cpf);

        PessoaFisicaResponse GetByNome(string nome);

        PessoaFisicaResponse GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps);

        PessoaFisicaResponse SaveData(PessoaFisicaCreateRequest? createRequest = null, PessoaFisicaUpdateRequest? updateRequest = null);
    }
}