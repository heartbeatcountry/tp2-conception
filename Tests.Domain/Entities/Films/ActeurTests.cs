using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using Moq;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class ActeurTests : PersonneTests<Acteur>
{
	protected override Acteur Entite { get; set; } = null!;

	[Test]
	public void AfterInstantiation_JoueDansFilmsShouldBeEmpty()
	{
		Assert.That(Entite.JoueDansFilms, Is.Empty);
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenFilmShouldBeAddedToJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>();

		// Act
		Entite.AjouterFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.Member(film));
	}

	[Test]
	public void GivenFilmToRemove_WhenRetirerFilm_ThenFilmShouldBeRemovedFromJoueDansFilms()
	{
		// Arrange
		var film = Mock.Of<IFilm>();
		Entite.AjouterFilm(film);

		// Act
		Entite.RetirerFilm(film);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.No.Member(film));
	}

	[Test]
	public void GivenFilmsToAdd_WhenAjouterFilms_ThenFilmsShouldBeAddedToJoueDansFilms()
	{
		// Arrange
		var films = new List<IFilm>
		{
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
			Mock.Of<IFilm>(m => m.Id == Guid.NewGuid()),
		};

		// Act
		Entite.AjouterFilms(films);

		// Assert
		Assert.That(Entite.JoueDansFilms, Has.Member(films[0]));
		Assert.That(Entite.JoueDansFilms, Has.Member(films[1]));
	}

	[Test]
	public void GivenFilmToAdd_WhenAjouterFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmInJoueDansFilms_WhenRetirerFilm_ThenReturnTrue()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void GivenFilmNotInJoueDansFilms_WhenRetirerFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());

		// Act
		var result = Entite.RetirerFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}

	[Test]
	public void GivenFilmAlreadyInJoueDansFilms_WhenAjouterFilm_ThenReturnFalse()
	{
		// Arrange
		var film = Mock.Of<IFilm>(m => m.Id == Guid.NewGuid());
		Entite.AjouterFilm(film);

		// Act
		var result = Entite.AjouterFilm(film);

		// Assert
		Assert.That(result, Is.False);
	}
}