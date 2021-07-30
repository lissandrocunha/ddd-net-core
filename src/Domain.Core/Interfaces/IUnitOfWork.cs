using Domain.Core.Commands;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Exception LastException { get; }
        IDbContextTransaction Transaction { get; }

        bool Commit();
        void RollBack();
        T Repository<T>() where T : class;
    }

    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : class
    {
    };

}
