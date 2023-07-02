namespace ARVTech.DataAccess.Repository.Interfaces.EquHos
{
    using ARVTech.DataAccess.Core.Entities.EquHos;
    using ARVTech.DataAccess.Repository.Interfaces.Actions;

    /// <summary>
    /// 
    /// </summary>
    public interface IPessoaRepository : ICreateRepository<Pessoa>, IReadRepository<Pessoa, int>, IUpdateRepository<Pessoa>, IDeleteRepository<int>
    {
    }
}