﻿namespace ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaRepository : ICreateRepository<MatriculaEntity>, IReadRepository<MatriculaEntity, Guid>, IUpdateRepository<MatriculaEntity, Guid, MatriculaEntity>, IDeleteRepository<Guid>
    {
        void DeleteEspelhosPonto(Guid guidMatricula);

        MatriculaEntity GetByMatricula(string matricula);
    }
}