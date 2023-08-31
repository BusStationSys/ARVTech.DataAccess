using ARVTech.DataAccess.Core.Entities.UniPayCheck;
using ARVTech.DataAccess.Repository.Interfaces.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARVTech.DataAccess.Repository.Interfaces.UniPayCheck
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMatriculaEspelhoPontoCalculoRepository : ICreateRepository<MatriculaEspelhoPontoCalculoEntity>, IReadRepository<MatriculaEspelhoPontoCalculoEntity, Guid>, IDeleteRepository<Guid>
    {
        MatriculaEspelhoPontoCalculoEntity GetByGuidMatriculaEspelhoPontoAndIdCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo);
    }
}