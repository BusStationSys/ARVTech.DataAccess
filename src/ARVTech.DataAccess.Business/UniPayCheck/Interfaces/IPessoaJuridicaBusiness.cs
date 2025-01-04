namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public interface IPessoaJuridicaBusiness
    {
        void Delete(Guid guid);

        PessoaJuridicaResponseDto Get(Guid guid);

        public IEnumerable<PessoaJuridicaResponseDto> GetAll();

        PessoaJuridicaResponseDto GetByCnpj(string cnpj);

        PessoaJuridicaResponseDto GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaResponseDto GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        ExecutionResponseDto<PessoaJuridicaResponseDto> Import(EmpregadorResult pessoaJuridicaResult);

        ResumoImportacaoEmpregadoresResponseDto ImportFileEmpregadores(string content);

        PessoaJuridicaResponseDto SaveData(PessoaJuridicaRequestCreateDto? createDto = null, PessoaJuridicaRequestUpdateDto? updateDto = null);
    }
}