namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IUsuarioRepository : ICreateRepository<UsuarioEntity>, IReadRepository<UsuarioEntity, Guid>, IUpdateRepository<UsuarioEntity>, IDeleteRepository<Guid>
    {
        UsuarioEntity Autenticar(string apelidousuario, string email, string senha);

        bool ExisteApelidoUsuarioDuplicado(Guid guid, string apelidoUsuario);

        bool ExisteCPFDuplicado(Guid guid, string cpf);

        bool ExisteEmailDuplicado(Guid guid, string email);

        IEnumerable<UsuarioEntity> GetAll(Guid guidConta, int perfil);

        UsuarioEntity GetByApelidoUsuario(string apelidoUsuario);

        UsuarioEntity GetByEmail(string email);

        UsuarioEntity TrocarContaECabanhaLogados(Guid guid, Guid guidConta, Guid guidCabanha);


    }
}