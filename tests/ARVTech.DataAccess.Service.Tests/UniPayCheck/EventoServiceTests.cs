namespace ARVTech.DataAccess.Service.Tests.UniPayCheck
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck;
    using AutoMapper;
    using FluentAssertions;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class EventoServiceTests : IDisposable
    {
        private bool _disposedValue;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUnitOfWorkAdapter> _unitOfWorkAdapterMock;
        private readonly Mock<IUnitOfWorkRepositoryUniPayCheck> _unitOfWorkRepositoryUniPayCheckMock;
        private readonly Mock<IEventoRepository> _eventoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly EventoService _eventoService;

        private readonly int _id = 8;
        private readonly string _descricao = "REPOUSO REMUNERADO";

        public EventoServiceTests()
        {
            this._unitOfWorkMock = new Mock<IUnitOfWork>();
            this._unitOfWorkAdapterMock = new Mock<IUnitOfWorkAdapter>();
            this._unitOfWorkRepositoryUniPayCheckMock = new Mock<IUnitOfWorkRepositoryUniPayCheck>();
            this._eventoRepositoryMock = new Mock<IEventoRepository>();
            this._mapperMock = new Mock<IMapper>();

            this._unitOfWorkRepositoryUniPayCheckMock.Setup(r => r.EventoRepository).Returns(this._eventoRepositoryMock.Object);
            this._unitOfWorkAdapterMock.Setup(a => a.RepositoriesUniPayCheck).Returns(this._unitOfWorkRepositoryUniPayCheckMock.Object);
            this._unitOfWorkMock.Setup(u => u.Create(It.IsAny<int?>(), It.IsAny<string>())).Returns(this._unitOfWorkAdapterMock.Object);

            this._eventoService = new EventoService(
                this._unitOfWorkMock.Object,
                this._mapperMock.Object);
        }

        [Fact]
        public void Get_WhenIdIsValid_ShouldReturnMappedDto()
        {
            // Arrange
            var entity = new EventoEntity();

            var dto = new EventoResponseDto();

            this._eventoRepositoryMock.Setup(r => r.Get(
                this._id)).Returns(entity);

            this._mapperMock.Setup(m => m.Map<EventoResponseDto>(entity)).Returns(dto);

            // Act
            var result = this._eventoService.Get(
                this._id);

            // Assert
            result.Should().Be(dto);

            this._eventoRepositoryMock.Verify(r => r.Get(
                this._id), Times.Once);
        }

        [Fact]
        public void Get_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            this._eventoRepositoryMock.Setup(r => r.Get(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._eventoService.Get(99);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Delete_WhenIdIsValid_ShouldBeginCommitAndCallRepository()
        {
            // Act
            this._eventoService.Delete(10);

            // Assert
            this._unitOfWorkAdapterMock.Verify(a => a.BeginTransaction(), Times.Once);
            this._eventoRepositoryMock.Verify(r => r.Delete(10), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Delete_WhenRepositoryThrows_ShouldRollbackAndRethrow()
        {
            // Arrange
            this._unitOfWorkAdapterMock.Setup(a => a.Transaction).Returns(new Mock<IDbTransaction>().Object);
            this._eventoRepositoryMock.Setup(r => r.Delete(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._eventoService.Delete(10);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Once);
        }

        [Fact]
        public void Delete_WhenRepositoryThrowsAndTransactionIsNull_ShouldNotRollback()
        {
            // Arrange
            this._eventoRepositoryMock.Setup(r => r.Delete(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._eventoService.Delete(10);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Never);
        }

        [Fact]
        public void SaveData_WhenDtoHasId_ShouldGetUpdateAndCommit()
        {
            // Arrange
            var dto = new EventoRequestDto
            {
                Id = this._id,
                Descricao = this._descricao,
            };

            var existingDto = new EventoResponseDto();
            var responseDto = new EventoResponseDto();

            var entityFromGet = new EventoEntity
            {
                Id = this._id,
                Descricao = this._descricao,
            };

            var entityFromUpdate = new EventoEntity
            {
                Id = this._id,
                Descricao = this._descricao,
            };

            // Get() interno
            this._eventoRepositoryMock.Setup(r => r.Get(this._id)).Returns(entityFromGet);
            this._mapperMock.Setup(m => m.Map<EventoResponseDto>(entityFromGet)).Returns(existingDto);

            // SaveData() — branch Update
            this._mapperMock.Setup(m => m.Map<EventoEntity>(entityFromGet)).Returns(entityFromUpdate);
            this._eventoRepositoryMock.Setup(r => r.Update(entityFromUpdate.Id, entityFromUpdate)).Returns(entityFromUpdate);
            this._mapperMock.Setup(m => m.Map<EventoResponseDto>(entityFromUpdate)).Returns(responseDto);

            // Act
            var result = this._eventoService.SaveData(dto);

            // Assert
            result.Should().Be(responseDto);
            this._eventoRepositoryMock.Verify(r => r.Update(entityFromUpdate.Id, entityFromUpdate), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenDtoHasNoId_ShouldGetLastIdCreateAndCommit()
        {
            // Arrange — dto.Id nulo → branch Create
            var dto = new EventoRequestDto { Id = null };
            var entity = new EventoEntity { Id = this._id };
            var responseDto = new EventoResponseDto();

            this._eventoRepositoryMock.Setup(r => r.GetLastId()).Returns(this._id);
            this._mapperMock.Setup(m => m.Map<EventoEntity>(dto)).Returns(entity);
            this._eventoRepositoryMock.Setup(r => r.Create(entity)).Returns(entity);
            this._mapperMock.Setup(m => m.Map<EventoResponseDto>(entity)).Returns(responseDto);

            // Act
            var result = this._eventoService.SaveData(dto);

            // Assert
            result.Should().Be(responseDto);
            this._eventoRepositoryMock.Verify(r => r.GetLastId(), Times.Once);
            this._eventoRepositoryMock.Verify(r => r.Create(entity), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenRepositoryThrows_WithTransaction_ShouldRollbackAndRethrow()
        {
            // Arrange
            var dto = new EventoRequestDto { Id = null };

            this._unitOfWorkAdapterMock.Setup(a => a.Transaction).Returns(new Mock<IDbTransaction>().Object);
            this._eventoRepositoryMock.Setup(r => r.GetLastId()).Returns(this._id);
            this._mapperMock.Setup(m => m.Map<EventoEntity>(dto)).Returns(new EventoEntity());
            this._eventoRepositoryMock.Setup(r => r.Create(It.IsAny<EventoEntity>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._eventoService.SaveData(dto);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenRepositoryThrows_WithoutTransaction_ShouldRethrowWithoutRollback()
        {
            // Arrange — Transaction não configurado = Moq retorna null
            var dto = new EventoRequestDto { Id = null };

            this._eventoRepositoryMock.Setup(r => r.GetLastId()).Returns(this._id);
            this._mapperMock.Setup(m => m.Map<EventoEntity>(dto)).Returns(new EventoEntity());
            this._eventoRepositoryMock.Setup(r => r.Create(It.IsAny<EventoEntity>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._eventoService.SaveData(dto);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Never);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EventoServiceTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}