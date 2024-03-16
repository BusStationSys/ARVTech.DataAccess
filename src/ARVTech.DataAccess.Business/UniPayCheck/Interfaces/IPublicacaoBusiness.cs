namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IPublicacaoBusiness
    {
        void Delete(int id);

        PublicacaoResponseDto Get(int id);

        IEnumerable<PublicacaoResponseDto> GetAll();

        PublicacaoResponseDto GetImage(int id);

        IEnumerable<PublicacaoResponseDto> GetSobreNos(string dataAtualString);

        PublicacaoResponseDto SaveData(PublicacaoRequestCreateDto? createDto = null, PublicacaoRequestUpdateDto? updateDto = null);
    }
}