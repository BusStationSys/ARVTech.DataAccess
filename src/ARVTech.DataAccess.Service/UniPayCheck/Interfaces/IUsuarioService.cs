namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IUsuarioService
    {
        IEnumerable<UsuarioResponseDto> GetByUsername(string cpfEmailUsername);

        IEnumerable<UsuarioNotificacaoResponseDto> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null);

        UsuarioResponseDto CheckPasswordValid(Guid guid, string password);

        UsuarioResponseDto SaveData(UsuarioRequestCreateDto? createDto = null, UsuarioRequestUpdateDto? updateDto = null);
    }
}