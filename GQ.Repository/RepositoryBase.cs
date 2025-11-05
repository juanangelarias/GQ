using AutoMapper;
using GQ.Common.Dto;
using GQ.Common.Model;
using GQ.Database;
using GQ.Entities;
using Microsoft.EntityFrameworkCore;

namespace GQ.Repository;

public interface IRepositoryBase<in TEntity, TDto>
    where TEntity : class, IEntityBase
    where TDto : class, IDtoBase
{
    Task<TDto> CreateAsync( TDto dto );
    Task<IEnumerable<TDto>> CreateManyAsync( IEnumerable<TDto> dtos );
    Task<bool> DeleteAsync( long id );
    Task<bool> DeleteManyAsync( IEnumerable<long> ids );
    Task<IEnumerable<TDto>> GetAsync( IQueryable<TEntity>? query = null );
    Task<PagedResponse<TDto>> GetAsync( QueryParams parameters, IQueryable<TEntity>? query = null );
    (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>( QueryParams parameters, IQueryable<T>? query = null );
    Task<TDto> GetByIdAsync( long id );
    Task<TDto?> UpdateAsync( TDto dto, TEntity? existingRecord = null );
    Task<IEnumerable<TDto>> UpdateManyAsync( IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null );
    Task<PagedResponse<TDto>> GetPageAsync(QueryParams parameters);
}

public class RepositoryBase<TEntity, TDto>(IMapper mapper, DataContext db) : IRepositoryBase<TEntity, TDto>
    where TEntity : class, IEntityBase
    where TDto : class, IDtoBase
{
    private readonly DbContext _db = db;

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var newRecord = mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(newRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<TDto>(newRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> CreateManyAsync(IEnumerable<TDto> dtos)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var newRecords = dtos
                .Select(mapper.Map<TEntity>)
                .ToList();

            await _db.Set<TEntity>().AddRangeAsync(newRecords);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<IEnumerable<TDto>>(newRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteAsync(long id)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException();
            }

            _db.Set<TEntity>().Remove(existingRecord);
            
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeleteManyAsync(IEnumerable<long> ids)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();
            
            _db.Set<TEntity>().RemoveRange(existingRecords);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAsync(IQueryable<TEntity>? query = null)
    {
        var qry = query ?? GetQuery();

        var queryResult = await qry
            .ToListAsync();
        return mapper.Map<IEnumerable<TDto>>(queryResult);
    }

    public virtual async Task<PagedResponse<TDto>> GetAsync(QueryParams parameters, IQueryable<TEntity>? query = null)
    {
        var q = query ?? GetQuery();
        var queryable = GetPaginatedQueryable(parameters, q);

        var result = await queryable.query!
            .ToListAsync();
        var mappedResult = mapper.Map<IEnumerable<TDto>>(result);
        return new PagedResponse<TDto>(mappedResult, queryable.rowCount);
    }

    public (IQueryable<T>? query, int rowCount) GetPaginatedQueryable<T>(QueryParams parameters, IQueryable<T>? qry)
    {
        if (parameters.PageSize < 1)
        {
            throw new ArgumentException("Page Size cannot be 0");
        }
        
        if (parameters.PageIndex < 0)
        {
            return (null, 0);
        }
        
        var rowCount = (qry?
            .Count()) ?? 0;

        var skip = rowCount <= parameters.PageSize ? 0 : parameters.PageSize * parameters.PageIndex;

        var pagedQuery = qry?
            .Skip(skip)
            .Take(parameters.PageSize);

        return (pagedQuery, rowCount);
    }
    
    public virtual async Task<TDto> GetByIdAsync(long id)
    {
        var record = await GetQuery()
            .FirstOrDefaultAsync(x => x.Id == id);

        return mapper.Map<TDto>(record);
    }

    public virtual async Task<TDto?> UpdateAsync(TDto dto, TEntity? existingRecord = null)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            existingRecord ??= await GetQuery()
                .FirstOrDefaultAsync(record => record.Id == dto.Id);

            if (existingRecord == null)
            {
                return null;
            }

            mapper.Map(dto, existingRecord);
            _db.Update(existingRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<TDto>(existingRecord);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<TDto> UpdateAsync(TDto dto, IQueryable<TEntity> trackingQuery)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await trackingQuery.FirstOrDefaultAsync();
            mapper.Map(dto, existingRecord);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            var updatedRecord = await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);

            return mapper.Map<TDto>(updatedRecord);
        }
        catch
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<IEnumerable<TDto>> UpdateManyAsync(IEnumerable<TDto> dtos, IEnumerable<TEntity>? existingRecords = null)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            existingRecords ??= await GetQuery()
                .Where(record => dtos.Select(dto => dto.Id).Contains(record.Id))
                .ToListAsync();

            var baseEntities = existingRecords
                .ToList();
            mapper.Map(dtos, baseEntities);

            _db.UpdateRange(baseEntities);
            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return mapper.Map<IEnumerable<TDto>>(existingRecords);
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual Task<PagedResponse<TDto>> GetPageAsync(QueryParams parameters)
    {
        var response = new PagedResponse<TDto>([], 0);
        
        return Task.FromResult(response);
    }

    protected virtual IQueryable<TEntity> GetQuery()
    {
        return _db.Set<TEntity>()
                .AsNoTracking();
    }

    public virtual async Task<bool> DeletePermanentAsync(long id)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecord = await GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            if (existingRecord == null)
            {
                return false;
            }
            _db.Set<TEntity>().Remove(existingRecord);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    public virtual async Task<bool> DeletePermanentManyAsync(IEnumerable<long> ids)
    {
        await using var t = await _db.Database.BeginTransactionAsync();

        try
        {
            var existingRecords = await GetQuery()
                .Where(record => ids.Contains(record.Id)).ToListAsync();

            _db.Set<TEntity>().RemoveRange(existingRecords);

            await _db.SaveChangesAsync();
            await t.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await t.RollbackAsync();
            throw;
        }
    }

    //Detach all entities
    public void DetachAllEntities()
    {
        var changedEntriesCopy = _db.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || 
                      e.State == EntityState.Deleted || 
                      e.State == EntityState.Modified ||
                      e.State == EntityState.Unchanged)
            .ToList();

        
        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }
}