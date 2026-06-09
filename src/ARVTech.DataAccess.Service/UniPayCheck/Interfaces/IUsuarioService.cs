namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Create;
    using ARVTech.DataAccess.Contracts.PayCheck.Requests.Update;
    using ARVTech.DataAccess.Contracts.PayCheck.Responses;

    public interface IUsuarioService
    {
        IEnumerable<UsuarioResponse> GetByUsername(string cpfEmailUsername);

        IEnumerable<UsuarioNotificacaoResponse> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null);

        UsuarioResponse CheckPasswordValid(Guid guid, string password);

        UsuarioResponse SaveData(UsuarioCreateRequest? createRequest = null, UsuarioUpdateRequest? updateRequest = null);
    }
}