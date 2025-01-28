namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IUsuarioService
    {
        IEnumerable<UsuarioResponseDto> GetByUsername(string cpfEmailUsername);

        UsuarioResponseDto CheckPasswordValid(Guid guid, string password);

        UsuarioResponseDto SaveData(UsuarioRequestCreateDto? createDto = null, UsuarioRequestUpdateDto? updateDto = null);
    }
}