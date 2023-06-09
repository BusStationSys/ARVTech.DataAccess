namespace ARVTech.DataAccess.Business
{
    using System;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using AutoMapper;

    public abstract class BaseBusiness : IDisposable
    {
        private bool _disposedValue = false;

        protected IUnitOfWork _unitOfWork = null;

        protected Mapper _mapper = null;

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return this._unitOfWork;
            }
        }

        protected Mapper Mapper
        {
            get
            {
                return this._mapper;
            }
        }

        protected BaseBusiness(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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