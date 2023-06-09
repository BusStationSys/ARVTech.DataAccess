﻿namespace ARVTech.DataAccess.Repository.Interfaces.Actions
{
    using System.Collections.Generic;

    public interface IReadRepository<out T, Y> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(Y id);
    }
}