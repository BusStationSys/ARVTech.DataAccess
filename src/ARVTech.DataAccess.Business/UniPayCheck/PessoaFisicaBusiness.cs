namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class PessoaFisicaBusiness : BaseBusiness
    {
        // To detect redundant calls.
        private bool _disposedValue = false;

        public PessoaFisicaBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PessoaRequestDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        public void Delete(Guid guid)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Delete(
                    guid);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public PessoaFisicaRequestDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Get(
                        guid);

                    return this._mapper.Map<PessoaFisicaRequestDto>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PessoaFisicaRequestDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetAll();

                    return this._mapper.Map<IEnumerable<PessoaFisicaRequestDto>>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public PessoaFisicaResponseDto GetByNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetByNome(
                        nome);

                    return this._mapper.Map<PessoaFisicaResponseDto>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="numeroCtps"></param>
        /// <param name="serieCtps"></param>
        /// <param name="ufCtps"></param>
        /// <returns></returns>
        public PessoaFisicaResponseDto GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps)
        {
            try
            {
                if (string.IsNullOrEmpty(nome))
                    throw new ArgumentNullException(
                        nameof(
                            nome));
                else if (string.IsNullOrEmpty(numeroCtps))
                    throw new ArgumentNullException(
                        nameof(
                            numeroCtps));
                else if (string.IsNullOrEmpty(serieCtps))
                    throw new ArgumentNullException(
                        nameof(
                            serieCtps));
                else if (string.IsNullOrEmpty(ufCtps))
                    throw new ArgumentNullException(
                        nameof(
                            ufCtps));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.GetByNomeNumeroCtpsSerieCtpsAndUfCtps(
                        nome,
                        numeroCtps,
                        serieCtps,
                        ufCtps);

                    return this._mapper.Map<PessoaFisicaResponseDto>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PessoaFisicaResponseDto SaveData(PessoaFisicaRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<PessoaFisicaEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null &&
                    dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Update(
                        entity.Guid,
                        entity);
                }
                else
                {
                    entity.Pessoa = connection.RepositoriesUniPayCheck.PessoaRepository.Create(
                        entity.Pessoa);

                    entity.GuidPessoa = entity.Pessoa.Guid;

                    entity = connection.RepositoriesUniPayCheck.PessoaFisicaRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<PessoaFisicaResponseDto>(
                    entity);
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Protected implementation of Dispose pattern. https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        protected override void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    //  TODO: dispose managed state (managed objects).
                }

                this._disposedValue = true;
            }

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}