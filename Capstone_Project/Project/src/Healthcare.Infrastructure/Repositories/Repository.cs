// File: src/Healthcare.Infrastructure/Repositories/Repository.cs
// Layer: Infrastructure/data-access layer
// Purpose: This file is the generic repository implementation that centralizes common EF Core CRUD operations.
// Security note: data access should use EF Core or parameterized SQL to avoid SQL injection.
// Change note: Only documentation comments were added; executable logic and project behavior remain unchanged.

// Detailed comment update: important declarations and executable blocks below are explained inline for viva/demo preparation.
// File role: Repository file: isolates data-access queries from controllers and services.

// Required namespaces are imported here so this file can use framework and project classes.
using System.Collections.Generic;
using System.Threading.Tasks;
using Healthcare.Core.Entities;
// Required namespaces are imported here so this file can use framework and project classes.
using Healthcare.Core.Interfaces;
using Healthcare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

// Namespace keeps related classes organized according to the project layer/folder structure.
namespace Healthcare.Infrastructure.Repositories
{
    /// <summary>Repository belongs to the healthcare layered architecture and keeps responsibilities separated.</summary>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly HealthcareDbContext Context;
        protected readonly DbSet<T> DbSet;

        ///  <summary>Constructor receives dependencies from ASP.NET Core dependency injection instead of creating them manually.</summary>
        public Repository(HealthcareDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        /// <summary>Executes the GetAllAsync workflow while keeping the logic inside the correct project layer.</summary>
        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // ToListAsync executes the query asynchronously and materializes the result for display/API output.
            return await DbSet.AsNoTracking().ToListAsync();
        }

        /// <summary>Executes the GetByIdAsync workflow while keeping the logic inside the correct project layer.</summary>
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>Executes the AddAsync workflow while keeping the logic inside the correct project layer.</summary>
        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        /// <summary>Executes the Update workflow while keeping the logic inside the correct project layer.</summary>
        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        /// <summary>Handles removal or deactivation of a selected record.</summary>
        public virtual void Delete(T entity)
        {
            // Soft delete protects clinical history from accidental loss.
            entity.IsActive = false;
            DbSet.Update(entity);
        }
    }
}
