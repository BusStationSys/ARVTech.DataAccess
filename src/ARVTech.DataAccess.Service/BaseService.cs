namespace ARVTech.DataAccess.Service
{
    using System;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using AutoMapper;

    public abstract class BaseService : IDisposable
    {
        private bool _disposedValue = false;

        protected IUnitOfWork _unitOfWork;

        protected IMapper _mapper;

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return this._unitOfWork;
            }
        }

        protected IMapper Mapper
        {
            get
            {
                return this._mapper;
            }
        }

        protected BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;

            this._mapper = mapper;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }
    }
}