namespace ARVTech.DataAccess.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public abstract class BaseService : IDisposable
    {
        private bool _disposedValue = false;

        protected IUnitOfWork _unitOfWork;

        protected IMapper _mapper;

        [ExcludeFromCodeCoverage]
        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return this._unitOfWork;
            }
        }

        [ExcludeFromCodeCoverage]
        protected IMapper Mapper
        {
            get
            {
                return this._mapper;
            }
        }

        [ExcludeFromCodeCoverage]
        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;

            this._mapper = mapper;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [ExcludeFromCodeCoverage]
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
    }
}