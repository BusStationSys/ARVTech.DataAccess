namespace ARVTech.DataAccess.Service.Tests.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Domain.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.DataAccess.Service.UniPayCheck;
    using ARVTech.Shared.Security.Interfaces;
    using AutoMapper;
    using FluentAssertions;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UsuarioServiceTests : IDisposable
    {
        private bool _disposedValue;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUnitOfWorkAdapter> _unitOfWorkAdapterMock;
        private readonly Mock<IUnitOfWorkRepositoryUniPayCheck> _unitOfWorkRepositoryUniPayCheckMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        private readonly UsuarioService _usuarioService;

        private readonly Guid _guid = new(
            "C7E5A367-6D97-49F0-9F7E-9BB894E0D44F");

        private readonly string _username = "UserMain";

        private readonly string _password = "P@ssw0rd";

        public UsuarioServiceTests()
        {
            this._unitOfWorkMock = new Mock<IUnitOfWork>();
            this._unitOfWorkAdapterMock = new Mock<IUnitOfWorkAdapter>();
            this._unitOfWorkRepositoryUniPayCheckMock = new Mock<IUnitOfWorkRepositoryUniPayCheck>();
            this._usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._passwordHasherMock = new Mock<IPasswordHasher>();

            this._unitOfWorkRepositoryUniPayCheckMock.Setup(r => r.UsuarioRepository).Returns(this._usuarioRepositoryMock.Object);
            this._unitOfWorkAdapterMock.Setup(a => a.RepositoriesUniPayCheck).Returns(this._unitOfWorkRepositoryUniPayCheckMock.Object);
            this._unitOfWorkMock.Setup(u => u.Create(It.IsAny<int?>(), It.IsAny<string>())).Returns(this._unitOfWorkAdapterMock.Object);

            this._usuarioService = new UsuarioService(
                this._unitOfWorkMock.Object,
                this._mapperMock.Object,
                this._passwordHasherMock.Object);
        }

        //[Fact]
        //public void CheckPasswordValid_WhenGuidAndPasswordAreValid_ShouldReturnMappedDto()
        //{
        //    // Arrange
        //    var entity = new UsuarioEntity();
        //    var dto = new UsuarioResponseDto();

        //    this._usuarioRepositoryMock.Setup(r => r.CheckPasswordValid(
        //        this._guid,
        //        this._password)).Returns(entity);

        //    this._mapperMock.Setup(m => m.Map<UsuarioResponseDto>(
        //        entity)).Returns(
        //            dto);

        //    // Act
        //    var result = this._usuarioService.CheckPasswordValid(
        //        this._guid,
        //        this._password);

        //    // Assert
        //    result.Should().Be(dto);

        //    this._usuarioRepositoryMock.Verify(r => r.CheckPasswordValid(
        //            this._guid,
        //            this._password),
        //        Times.Once);
        //}

        [Fact]
        public void CheckPasswordValid_WhenGuidIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.CheckPasswordValid(
                Guid.Empty,
                this._password);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName(
                "guid");
        }

        [Fact]
        public void CheckPasswordValid_WhenPasswordIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.CheckPasswordValid(
                this._guid,
                null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName(
                "password");
        }

        [Fact]
        public void CheckPasswordValid_WhenPasswordIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.CheckPasswordValid(
                this._guid,
                string.Empty);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName(
                "password");
        }

        //[Fact]
        //public void CheckPasswordValid_WhenRepositoryThrows_ShouldRethrow()
        //{
        //    // Arrange
        //    this._usuarioRepositoryMock.Setup(r => r.CheckPasswordValid(
        //        It.IsAny<Guid>(),
        //        It.IsAny<string>())).Throws<InvalidOperationException>();

        //    // Act
        //    var act = () => this._usuarioService.CheckPasswordValid(
        //        this._guid,
        //        this._password);

        //    // Assert
        //    act.Should().Throw<InvalidOperationException>();
        //}

        [Fact]
        public void Get_WhenIdIsValid_ShouldReturnMappedDto()
        {
            // Arrange
            var entity = new UsuarioEntity();

            var dto = new UsuarioResponseDto();

            this._usuarioRepositoryMock.Setup(r => r.Get(
                this._guid)).Returns(entity);

            this._mapperMock.Setup(m => m.Map<UsuarioResponseDto>(
                entity)).Returns(
                    dto);

            // Act
            var result = this._usuarioService.Get(
                this._guid);

            // Assert
            result.Should().Be(dto);

            this._usuarioRepositoryMock.Verify(r => r.Get(
                    this._guid),
                Times.Once);
        }

        [Fact]
        public void Get_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            this._usuarioRepositoryMock.Setup(r => r.Get(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.Get(
                this._guid);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Delete_WhenGuidIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.Delete(
                Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("guid");
        }

        [Fact]
        public void Get_WhenGuidIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.Get(Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("guid");
        }

        [Fact]
        public void GetAll_WhenCalled_ShouldReturnMappedDtos()
        {
            // Arrange
            var entities = new List<UsuarioEntity>
            {
                new ()
            };

            var dtos = new List<UsuarioResponseDto>
            {
                new ()
            };

            this._usuarioRepositoryMock.Setup(r => r.GetAll()).Returns(
                entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(
                dtos);

            this._usuarioRepositoryMock.Verify(
                r => r.GetAll(),
                Times.Once);
        }

        [Fact]
        public void GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var entities = new List<UsuarioEntity>();
            var dtos = new List<UsuarioResponseDto>();

            this._usuarioRepositoryMock.Setup(r => r.GetAll())
                .Returns(entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioResponseDto>>(
                entities)).Returns(dtos);

            // Act
            var result = this._usuarioService.GetAll();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAll_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            this._usuarioRepositoryMock.Setup(r => r.GetAll()).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.GetAll();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetByUsername_WhenUsernameIsValid_ShouldReturnMappedDtos()
        {
            // Arrange
            var entities = new List<UsuarioEntity>
            {
                new ()
            };

            var dtos = new List<UsuarioResponseDto>
            {
                new ()
            };

            this._usuarioRepositoryMock.Setup(r => r.GetByUsername(
                this._username)).Returns(
                    entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetByUsername(
                this._username);

            // Assert
            result.Should().BeEquivalentTo(
                dtos);

            this._usuarioRepositoryMock.Verify(r => r.GetByUsername(
                    this._username),
                Times.Once);
        }

        [Fact]
        public void GetByUsername_WhenUsernameIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.GetByUsername(
                null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName(
                "cpfEmailUsername");
        }

        [Fact]
        public void GetByUsername_WhenUsernameIsEmpty_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => this._usuarioService.GetByUsername(
                string.Empty);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithParameterName(
                "cpfEmailUsername");
        }

        [Fact]
        public void GetByUsername_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var entities = new List<UsuarioEntity>();
            var dtos = new List<UsuarioResponseDto>();

            this._usuarioRepositoryMock.Setup(r => r.GetByUsername(
                this._username)).Returns(
                    entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetByUsername(
                this._username);

            // Assert
            result.Should().BeEmpty();

            this._usuarioRepositoryMock.Verify(r => r.GetByUsername(
                    this._username),
                Times.Once);
        }

        [Fact]
        public void GetByUsername_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            this._usuarioRepositoryMock.Setup(r => r.GetByUsername(
                It.IsAny<string>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.GetByUsername(
                this._username);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetNotificacoes_WhenCalledWithDefaultParameters_ShouldReturnMappedDtos()
        {
            // Arrange
            var entities = new List<UsuarioNotificacaoEntity>
            {
                new ()
            };

            var dtos = new List<UsuarioNotificacaoResponseDto>
            {
                new ()
            };

            this._usuarioRepositoryMock.Setup(r => r.GetNotificacoes(
                null, null, null, null, null)).Returns(
                    entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioNotificacaoResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetNotificacoes();

            // Assert
            result.Should().BeEquivalentTo(
                dtos);

            this._usuarioRepositoryMock.Verify(r => r.GetNotificacoes(
                    null, null, null, null, null),
                Times.Once);
        }

        [Fact]
        public void GetNotificacoes_WhenCalledWithAllParameters_ShouldPassThemToRepository()
        {
            // Arrange
            var tipo = "AVISO";
            var guidMatriculaDemonstrativoPagamento = Guid.NewGuid();
            var guidEmpregador = Guid.NewGuid();
            var guidColaborador = Guid.NewGuid();

            var entities = new List<UsuarioNotificacaoEntity>
            {
                new ()
            };

            var dtos = new List<UsuarioNotificacaoResponseDto>
            {
                new ()
            };

            this._usuarioRepositoryMock.Setup(r => r.GetNotificacoes(
                tipo, this._guid, guidMatriculaDemonstrativoPagamento, guidEmpregador, guidColaborador)).Returns(
                    entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioNotificacaoResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetNotificacoes(
                tipo, this._guid, guidMatriculaDemonstrativoPagamento, guidEmpregador, guidColaborador);

            // Assert
            result.Should().BeEquivalentTo(
                dtos);

            this._usuarioRepositoryMock.Verify(r => r.GetNotificacoes(
                    tipo, this._guid, guidMatriculaDemonstrativoPagamento, guidEmpregador, guidColaborador),
                Times.Once);
        }

        [Fact]
        public void GetNotificacoes_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var entities = new List<UsuarioNotificacaoEntity>();
            var dtos = new List<UsuarioNotificacaoResponseDto>();

            this._usuarioRepositoryMock.Setup(r => r.GetNotificacoes(
                null, null, null, null, null)).Returns(
                    entities);

            this._mapperMock.Setup(m => m.Map<IEnumerable<UsuarioNotificacaoResponseDto>>(
                entities)).Returns(
                    dtos);

            // Act
            var result = this._usuarioService.GetNotificacoes();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNotificacoes_WhenRepositoryThrows_ShouldRethrow()
        {
            // Arrange
            this._usuarioRepositoryMock.Setup(r => r.GetNotificacoes(
                It.IsAny<string>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.GetNotificacoes();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Delete_WhenGuidIsValid_ShouldBeginCommitAndCallRepository()
        {
            // Act
            this._usuarioService.Delete(
                this._guid);

            // Assert
            this._unitOfWorkAdapterMock.Verify(a => a.BeginTransaction(), Times.Once);
            this._usuarioRepositoryMock.Verify(r => r.Delete(this._guid), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Delete_WhenRepositoryThrows_ShouldRollbackAndRethrow()
        {
            // Arrange
            this._unitOfWorkAdapterMock.Setup(a => a.Transaction).Returns(new Mock<IDbTransaction>().Object);
            this._usuarioRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.Delete(this._guid);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Once);
        }

        [Fact]
        public void Delete_WhenRepositoryThrowsAndTransactionIsNull_ShouldNotRollback()
        {
            // Arrange
            this._usuarioRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.Delete(this._guid);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Never);
        }

        [Fact]
        public void SaveData_WhenBothDtosAreProvided_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var createDto = new UsuarioRequestCreateDto();

            var updateDto = new UsuarioRequestUpdateDto { 
                Guid = this._guid 
            };

            // Act
            var act = () => this._usuarioService.SaveData(
                createDto, updateDto);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*createDto*updateDto*");
        }

        [Fact]
        public void SaveData_WhenBothDtosAreNull_ShouldThrowInvalidOperationException()
        {
            // Act
            var act = () => this._usuarioService.SaveData(
                null, null);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*createDto*updateDto*");
        }

        [Fact]
        public void SaveData_WhenUpdateDtoHasEmptyGuid_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var updateDto = new UsuarioRequestUpdateDto { Guid = Guid.Empty };

            // Act
            var act = () => this._usuarioService.SaveData(
                null, updateDto);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Guid*");
        }

        [Fact]
        public void SaveData_WhenCreateDtoIsProvided_ShouldCreateAndReturnMappedDto()
        {
            // Arrange
            var createDto = new UsuarioRequestCreateDto { Username = this._username };
            var entity = new UsuarioEntity();
            var createdEntity = new UsuarioEntity();
            var dto = new UsuarioResponseDto();

            this._mapperMock.Setup(m => m.Map<UsuarioEntity>(
                createDto)).Returns(entity);

            this._usuarioRepositoryMock.Setup(r => r.Create(
                entity)).Returns(createdEntity);

            this._mapperMock.Setup(m => m.Map<UsuarioResponseDto>(
                createdEntity)).Returns(dto);

            // Act
            var result = this._usuarioService.SaveData(
                createDto, null);

            // Assert
            result.Should().Be(dto);

            this._unitOfWorkAdapterMock.Verify(a => a.BeginTransaction(), Times.Once);
            this._usuarioRepositoryMock.Verify(r => r.Create(entity), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenUpdateDtoIsProvided_ShouldUpdateAndReturnMappedDto()
        {
            // Arrange
            var updateDto = new UsuarioRequestUpdateDto { Guid = this._guid, Username = this._username };
            var entity = new UsuarioEntity { Guid = this._guid };
            var updatedEntity = new UsuarioEntity { Guid = this._guid };
            var dto = new UsuarioResponseDto();

            this._mapperMock.Setup(m => m.Map<UsuarioEntity>(
                updateDto)).Returns(entity);

            this._usuarioRepositoryMock.Setup(r => r.Update(
                this._guid, entity)).Returns(updatedEntity);

            this._mapperMock.Setup(m => m.Map<UsuarioResponseDto>(
                updatedEntity)).Returns(dto);

            // Act
            var result = this._usuarioService.SaveData(
                null, updateDto);

            // Assert
            result.Should().Be(dto);

            this._unitOfWorkAdapterMock.Verify(a => a.BeginTransaction(), Times.Once);
            this._usuarioRepositoryMock.Verify(r => r.Update(this._guid, entity), Times.Once);
            this._unitOfWorkAdapterMock.Verify(a => a.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenRepositoryThrowsAndTransactionIsNotNull_ShouldRollbackAndRethrow()
        {
            // Arrange
            var createDto = new UsuarioRequestCreateDto { Username = this._username };
            var entity = new UsuarioEntity();

            this._mapperMock.Setup(m => m.Map<UsuarioEntity>(
                createDto)).Returns(entity);

            this._usuarioRepositoryMock.Setup(r => r.Create(
                It.IsAny<UsuarioEntity>())).Throws<InvalidOperationException>();

            this._unitOfWorkAdapterMock.Setup(a => a.Transaction)
                .Returns(new Mock<IDbTransaction>().Object);

            // Act
            var act = () => this._usuarioService.SaveData(
                createDto, null);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            this._unitOfWorkAdapterMock.Verify(a => a.Rollback(), Times.Once);
        }

        [Fact]
        public void SaveData_WhenRepositoryThrowsAndTransactionIsNull_ShouldNotRollbackAndRethrow()
        {
            // Arrange
            var createDto = new UsuarioRequestCreateDto 
            { 
                Username = this._username 
            };

            var entity = new UsuarioEntity();

            this._mapperMock.Setup(m => m.Map<UsuarioEntity>(
                createDto)).Returns(
                    entity);

            this._usuarioRepositoryMock.Setup(r => r.Create(
                It.IsAny<UsuarioEntity>())).Throws<InvalidOperationException>();

            // Act
            var act = () => this._usuarioService.SaveData(
                createDto,
                null);

            // Assert
            act.Should().Throw<InvalidOperationException>();

            this._unitOfWorkAdapterMock.Verify(
                a => a.Rollback(),
                Times.Never);
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