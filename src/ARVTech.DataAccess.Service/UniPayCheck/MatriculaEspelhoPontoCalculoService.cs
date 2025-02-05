﻿namespace ARVTech.DataAccess.Service.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public class MatriculaEspelhoPontoCalculoService : BaseService
    {
        public MatriculaEspelhoPontoCalculoService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaEspelhoPontoCalculoRequestDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoCalculoResponseDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaEspelhoPontoRequestCreateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoRequestUpdateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();

                cfg.CreateMap<MatriculaRequestCreateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestUpdateDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();

                cfg.CreateMap<CalculoRequestDto, CalculoEntity>().ReverseMap();
                cfg.CreateMap<CalculoResponseDto, CalculoEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoCalculoRepository.Delete(
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
        public MatriculaEspelhoPontoCalculoResponseDto Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoCalculoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaEspelhoPontoCalculoResponseDto>(
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
        public IEnumerable<MatriculaEspelhoPontoCalculoResponseDto> GetAll()
        {
            try
            {
                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.GetAll();

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoCalculoResponseDto>>(
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
        /// <param name="idCalculo"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoCalculoResponseDto GetByGuidMatriculaEspelhoPontoAndIdCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo)
        {
            try
            {
                if (guidMatriculaEspelhoPonto == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatriculaEspelhoPonto));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoCalculoRepository.GetByGuidMatriculaEspelhoPontoAndIdCalculo(
                        guidMatriculaEspelhoPonto,
                        idCalculo);

                    return this._mapper.Map<MatriculaEspelhoPontoCalculoResponseDto>(
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
        public MatriculaEspelhoPontoCalculoResponseDto SaveData(MatriculaEspelhoPontoCalculoRequestDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaEspelhoPontoCalculoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    string x = string.Empty;

                    //entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoTotalizadorRepository.Update(
                    //    entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoCalculoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaEspelhoPontoCalculoResponseDto>(
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