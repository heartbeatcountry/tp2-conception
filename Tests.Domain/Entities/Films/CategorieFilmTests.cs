using CineQuebec.Domain.Entities.Films;
using Tests.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Films;

public class CategorieFilmTests : EntiteTests<CategorieFilm>
{
	private const string NomValide = "Action";
	private const string AutreNomValide = "Comédie";
	protected override object?[] ArgsConstructeur => [NomValide];

	[Test]
	public void CompareTo_WhenGivenNull_ShouldReturn1()
	{
		// Act
		var result = Entite.CompareTo(null);

		// Assert
		Assert.That(result, Is.EqualTo(1));
	}

	[Test]
	public void CompareTo_WhenGivenOtherInstanceWithDifferentNomAffichage_ShouldReturnComparisonResult()
	{
		// Arrange
		var autreCategorie = CreateInstance(AutreNomValide);

		// Act
		var result = Entite.CompareTo(autreCategorie);

		// Assert
		Assert.That(result,
			Is.EqualTo(string.Compare(Entite.NomAffichage, autreCategorie.NomAffichage, StringComparison.Ordinal)));
	}

	[Test]
	public void CompareTo_WhenGivenSameInstance_ShouldReturn0()
	{
		// Act
		var result = Entite.CompareTo(Entite);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void Constructor_WhenNomAffichageIsNullOrWhitespace_ShouldThrowArgumentException()
	{
		Assert.Multiple(() =>
		{
			Assert.That(() => CreateInstance(string.Empty), Throws.ArgumentException);
			Assert.That(() => CreateInstance(" "), Throws.ArgumentException);
		});
	}

	[Test]
	public void Constructor_WhenNomAffichageIsValid_ShouldSetNomAffichageToNomAffichageGivenInConstructor()
	{
		// Assert
		Assert.That(Entite.NomAffichage, Is.EqualTo(NomValide));
	}

	[Test]
	public void Equals_WhenGivenBoxedCategorieFilmWithSameNomAffichage_ShouldReturnTrue()
	{
		// Arrange
		var autreCategorie = CreateInstance(NomValide);
		autreCategorie.SetId(Guid.NewGuid());

		// Act
		var result = Entite.Equals(autreCategorie);

		// Assert
		Assert.That(result, Is.True);
	}

	[Test]
	public void SetNomAffichage_WhenGivenNomAffichageIsNullOrWhitespace_ShouldThrowArgumentException()
	{
		Assert.Multiple(() =>
		{
			Assert.That(() => Entite.SetNomAffichage(string.Empty), Throws.ArgumentException);
			Assert.That(() => Entite.SetNomAffichage(" "), Throws.ArgumentException);
		});
	}

	[Test]
	public void SetNomAffichage_WhenGivenValidNomAffichage_ShouldSetNomAffichageToGivenNomAffichage()
	{
		// Act
		Entite.SetNomAffichage(AutreNomValide);

		// Assert
		Assert.That(Entite.NomAffichage, Is.EqualTo(AutreNomValide));
	}

	[Test]
	public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo(Entite.NomAffichage));
	}
}