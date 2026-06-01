namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IUsuarioRepository : ICreateRepository<UsuarioEntity>, IReadRepository<UsuarioEntity, Guid>, IUpdateRepository<UsuarioEntity, Guid, UsuarioEntity>, IDeleteRepository<Guid>
    {
        IEnumerable<UsuarioEntity> GetByUsername(string cpfEmailUsername);

        IEnumerable<UsuarioNotificacaoEntity> GetNotificacoes(string tipo = null, Guid? guidUsuario = null, Guid? guidMatriculaDemonstrativoPagamento = null, Guid? guidEmpregador = null, Guid? guidColaborador = null);
    }
}