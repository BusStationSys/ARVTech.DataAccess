namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public interface IPessoaJuridicaService
    {
        void Delete(Guid guid);

        PessoaJuridicaResponse? Get(Guid guid);

        IEnumerable<PessoaJuridicaResponse> GetAll();

        PessoaJuridicaResponse GetByCnpj(string cnpj);

        PessoaJuridicaResponse GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaResponse GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        ResumoImportacaoEmpregadoresResponse ImportFileEmpregadores(string content);

        PessoaJuridicaResponse SaveData(PessoaJuridicaCreateRequest? createRequest = null, PessoaJuridicaUpdateRequest? updateRequest = null);
    }
}