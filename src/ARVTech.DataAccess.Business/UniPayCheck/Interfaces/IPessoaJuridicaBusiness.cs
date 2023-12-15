namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IPessoaJuridicaBusiness
    {
        void Delete(Guid guid);

        PessoaJuridicaResponseDto Get(Guid guid);

        public IEnumerable<PessoaJuridicaResponseDto> GetAll();

        PessoaJuridicaResponseDto GetByCnpj(string cnpj);

        PessoaJuridicaResponseDto GetByRazaoSocial(string razaoSocial);

        PessoaJuridicaResponseDto GetByRazaoSocialAndCnpj(string razaoSocial, string cnpj);

        PessoaJuridicaResponseDto SaveData(PessoaJuridicaRequestCreateDto? createDto = null, PessoaJuridicaRequestUpdateDto? updateDto = null);
    }
}