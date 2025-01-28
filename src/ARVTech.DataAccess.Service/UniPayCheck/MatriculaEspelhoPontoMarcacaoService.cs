namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class MatriculaEspelhoPontoMarcacaoService : BaseService
    {
        public MatriculaEspelhoPontoMarcacaoService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaEspelhoPontoMarcacaoRequestDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoMarcacaoResponseDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaEspelhoPontoRequestCreateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoRequestUpdateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoMarcacaoRepository.Delete(
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
        public MatriculaEspelhoPontoMarcacaoResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoMarcacaoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaEspelhoPontoMarcacaoResponseDto>(
                        entity);
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
        public IEnumerable<MatriculaEspelhoPontoMarcacaoResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoMarcacaoRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoMarcacaoResponseDto>>(
                        entity);
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
        /// <param name="guidMatriculaEspelhoPonto"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoMarcacaoResponseDto GetByGuidMatriculaEspelhoPontoAndData(Guid guidMatriculaEspelhoPonto, DateTime data)
        {
            try
            {
                if (guidMatriculaEspelhoPonto == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaEspelhoPonto));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoMarcacaoRepository.GetByGuidMatriculaEspelhoPontoAndData(
                        guidMatriculaEspelhoPonto,
                        data);

                    return this._mapper.Map<MatriculaEspelhoPontoMarcacaoResponseDto>(
                        entity);
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
        public MatriculaEspelhoPontoMarcacaoResponseDto SaveData(MatriculaEspelhoPontoMarcacaoRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaEspelhoPontoMarcacaoEntity>(
                    dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    string x = string.Empty;

                    //entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoEventoRepository.Update(
                    //    entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoMarcacaoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaEspelhoPontoMarcacaoResponseDto>(
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
    }
}