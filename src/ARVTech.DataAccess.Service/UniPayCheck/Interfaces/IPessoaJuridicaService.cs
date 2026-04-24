namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IPessoaJuridicaService
    {
        void Delete(Guid guid);

        PessoaJuridicaResponseDto Get(Guid guid);

        public IEnumerable<PessoaJuridicaResponseDto> GetAll();

        PessoaJuridicaResponseDto GetByCnpj(string cnpj);

        PessoaJuridicaResponseDto GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaResponseDto GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        ResumoImportacaoEmpregadoresResponseDto ImportFileEmpregadores(string content);

        PessoaJuridicaResponseDto SaveData(PessoaJuridicaRequestCreateDto? createDto = null, PessoaJuridicaRequestUpdateDto? updateDto = null);
    }
}