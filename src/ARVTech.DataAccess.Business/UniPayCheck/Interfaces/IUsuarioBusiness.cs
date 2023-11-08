namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IUsuarioBusiness
    {
        IEnumerable<UsuarioResponseDto> GetByUsername(string cpfEmailUsername);

        UsuarioResponseDto CheckPasswordValid(Guid guid, string password);

        UsuarioResponseDto SaveData(UsuarioRequestCreateDto? createDto = null, UsuarioRequestUpdateDto? updateDto = null);
    }
}