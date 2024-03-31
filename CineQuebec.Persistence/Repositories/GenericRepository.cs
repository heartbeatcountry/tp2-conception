using System.Collections.Immutable;
using System.Linq.Expressions;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineQuebec.Persistence.Repositories;

public class GenericRepository<TEntite> : IRepository<TEntite> where TEntite : Entite
{
	private readonly IApplicationDbContext _context;
	private readonly DbSet<TEntite> _dbSet;

	public GenericRepository(IApplicationDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<TEntite>();
	}

	public async Task<TEntite> AjouterAsync(TEntite entite)
	{
		var res = await _dbSet.AddAsync(entite).AsTask();
		return res.Entity;
	}

	public TEntite Modifier(TEntite entite)
	{
		_dbSet.Attach(entite);
		_context.Entry(entite).State = EntityState.Modified;
		return entite;
	}

	public async Task<TEntite?> ObtenirParIdAsync(Guid id)
	{
		return await _dbSet.FindAsync(id).AsTask();
	}

	public async Task<ImmutableArray<TEntite>> ObtenirTousAsync(Expression<Func<TEntite, bool>>? filtre = null,
		Func<IQueryable<TEntite>, IOrderedQueryable<TEntite>>? trierPar = null)
	{
		IQueryable<TEntite> query = _dbSet;

		if (filtre != null)
		{
			query = query.Where(filtre);
		}

		var arr = trierPar is not null ? await trierPar(query).ToArrayAsync() : await query.ToArrayAsync();
		return arr.ToImmutableArray();
	}

	public void Supprimer(TEntite entite)
	{
		if (_context.Entry(entite).State == EntityState.Detached)
		{
			_dbSet.Attach(entite);
		}

		_dbSet.Remove(entite);
	}

	public void Supprimer(Guid id)
	{
		if (_dbSet.Find(id) is { } entite)
		{
			Supprimer(entite);
		}
	}
}