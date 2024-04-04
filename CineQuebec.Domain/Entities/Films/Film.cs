using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Film : Entite, IComparable<Film>, IFilm
{
	private readonly HashSet<Guid> _acteurs = [];
	private readonly HashSet<Guid> _realisateurs = [];

	public Film(string titre, string description, Guid categorie, DateOnly dateSortieInternationale,
		IEnumerable<Guid> acteurs, IEnumerable<Guid> realisateurs, ushort dureeEnMinutes)
	{
		SetTitre(titre);
		SetDescription(description);
		SetCategorie(categorie);
		SetDateSortieInternationale(dateSortieInternationale);
		AddActeurs(acteurs);
		AddRealisateurs(realisateurs);
		SetDureeEnMinutes(dureeEnMinutes);
	}

	private Film(Guid id, string titre, string description, DateOnly dateSortieInternationale, ushort dureeEnMinutes)
	{
		SetId(id);
		SetTitre(titre);
		SetDescription(description);
		SetDateSortieInternationale(dateSortieInternationale);
		SetDureeEnMinutes(dureeEnMinutes);
	}

	public string Titre { get; private set; } = string.Empty;
	public string Description { get; private set; } = string.Empty;
	public virtual Guid IdCategorie { get; private set; }
	public DateOnly DateSortieInternationale { get; private set; } = DateOnly.MinValue;
	public virtual IEnumerable<Guid> ActeursParId => _acteurs.ToImmutableArray();
	public virtual IEnumerable<Guid> RealisateursParId => _realisateurs.ToImmutableArray();
	public ushort DureeEnMinutes { get; private set; }

	public void AddActeurs(IEnumerable<Guid> acteurs)
	{
		_acteurs.UnionWith(acteurs);
	}

	public void AddRealisateurs(IEnumerable<Guid> realisateurs)
	{
		_realisateurs.UnionWith(realisateurs);
	}

	public int CompareTo(Film? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		var dateComparison = DateSortieInternationale.CompareTo(other.DateSortieInternationale);
		return dateComparison != 0 ? dateComparison : string.Compare(Titre, other.Titre, StringComparison.Ordinal);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Film film &&
		                              string.Equals(Titre, film.Titre,
			                              StringComparison.OrdinalIgnoreCase) && DateSortieInternationale.Year ==
		                              film.DateSortieInternationale.Year && DureeEnMinutes == film.DureeEnMinutes);
	}

	public void SetActeurs(IEnumerable<Guid> acteurs)
	{
		_acteurs.Clear();
		AddActeurs(acteurs);
	}

	public void SetCategorie(Guid categorie)
	{
		if (categorie == Guid.Empty)
		{
			throw new ArgumentException("Le guid de la catégorie ne peut pas être nul.", nameof(categorie));
		}

		IdCategorie = categorie;
	}

	public void SetDateSortieInternationale(DateOnly dateSortieInternationale)
	{
		if (dateSortieInternationale == DateOnly.MinValue)
		{
			throw new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
				$"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.");
		}

		DateSortieInternationale = dateSortieInternationale;
	}

	public void SetDescription(string description)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentException("La description ne peut pas être vide.", nameof(description));
		}

		Description = description.Trim();
	}

	public void SetDureeEnMinutes(ushort duree)
	{
		if (duree == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes.");
		}

		DureeEnMinutes = duree;
	}

	public void SetRealisateurs(IEnumerable<Guid> realisateurs)
	{
		_realisateurs.Clear();
		AddRealisateurs(realisateurs);
	}

	public void SetTitre(string titre)
	{
		if (string.IsNullOrWhiteSpace(titre))
		{
			throw new ArgumentException("Le titre ne peut pas être vide.", nameof(titre));
		}

		Titre = titre.Trim();
	}

	public override string ToString()
	{
		return $"{Titre} ({DateSortieInternationale.Year})";
	}
}