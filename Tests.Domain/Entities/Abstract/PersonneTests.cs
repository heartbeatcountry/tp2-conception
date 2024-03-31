using CineQuebec.Domain.Entities.Abstract;

namespace Tests.Domain.Entities.Abstract;

public abstract class PersonneTests<T> : EntiteTests<T> where T : Personne
{
	private const string PrenomValide = "Michel";
	private const string AutrePrenomValide = "Ginette";
	private const string NomValide = "Sardou";
	private const string AutreNomValide = "Bergeron";
	protected override object?[] ArgsConstructeur => [PrenomValide, NomValide];

	[Test]
	public void CompareTo_WhenGivenDifferentInstanceWithDifferentNomComplet_ShouldReturnComparisonResult()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, AutreNomValide);
		autre.SetId(Guid.NewGuid());

		// Act & Assert
		Assert.Multiple(() =>
		{
			Assert.That(Entite.CompareTo(autre), Is.GreaterThan(0));
			Assert.That(autre.CompareTo(Entite), Is.LessThan(0));
		});
	}

	[Test]
	public void CompareTo_WhenGivenDifferentInstanceWithSameNomComplet_ShouldReturn0()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, NomValide);
		autre.SetId(Guid.NewGuid());

		// Act
		var result = Entite.CompareTo(autre);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void CompareTo_WhenGivenDifferentInstanceWithSameNomCompletInLowerCase_ShouldReturn0()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, NomValide.ToLower());

		// Act
		var result = Entite.CompareTo(autre);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void CompareTo_WhenGivenDifferentInstanceWithSameNomCompletInUpperCase_ShouldReturn0()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, NomValide.ToUpper());

		// Act
		var result = Entite.CompareTo(autre);

		// Assert
		Assert.That(result, Is.EqualTo(0));
	}

	[Test]
	public void CompareTo_WhenGivenNull_ShouldReturn1()
	{
		// Act
		var result = Entite.CompareTo(null);

		// Assert
		Assert.That(result, Is.EqualTo(1));
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
	public void Constructor_WhenGivenNomIsNullOrWhitespace_ShouldThrowArgumentException()
	{
		Assert.Multiple(() =>
		{
			Assert.That(() => CreateInstance(PrenomValide, string.Empty), Throws.ArgumentException);
			Assert.That(() => CreateInstance(PrenomValide, " "), Throws.ArgumentException);
		});
	}

	[Test]
	public void Constructor_WhenGivenNomIsValid_ShouldSetNomToGivenNom()
	{
		// Assert
		Assert.That(Entite.Nom, Is.EqualTo(NomValide));
	}

	[Test]
	public void Constructor_WhenGivenPrenomIsNullOrWhitespace_ShouldThrowArgumentException()
	{
		Assert.Multiple(() =>
		{
			Assert.That(() => CreateInstance(string.Empty, NomValide), Throws.ArgumentException);
			Assert.That(() => CreateInstance(" ", NomValide), Throws.ArgumentException);
		});
	}

	[Test]
	public void Constructor_WhenGivenPrenomIsValid_ShouldSetPrenomToGivenPrenom()
	{
		// Assert
		Assert.That(Entite.Prenom, Is.EqualTo(PrenomValide));
	}

	[Test]
	public void Equals_WhenGivenOtherInstanceWithDifferentNomCompletAndId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance("Michel", "Berger");
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite.Equals(autre), Is.False);
	}

	[Test]
	public void Equals_WhenGivenOtherInstanceWithSameNomCompletAndId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance(PrenomValide, NomValide);

		// Assert
		Assert.That(Entite.Equals(autre), Is.True);
	}

	[Test]
	public void NomComplet_Always_ShouldBeEqualToPrenomAndNom()
	{
		Assert.That(Entite.NomComplet, Is.EqualTo($"{PrenomValide} {NomValide}"));
	}

	[Test]
	public void SetPrenom_WhenGivenPrenomIsNullOrWhitespace_ShouldThrowArgumentException()
	{
		Assert.Multiple(() =>
		{
			Assert.That(() => Entite.SetPrenom(string.Empty), Throws.ArgumentException);
			Assert.That(() => Entite.SetPrenom(" "), Throws.ArgumentException);
		});
	}

	[Test]
	public void SetPrenom_WhenGivenValidPrenom_ShouldSetPrenomToGivenPrenom()
	{
		// Act
		Entite.SetPrenom(AutrePrenomValide);

		// Assert
		Assert.That(Entite.Prenom, Is.EqualTo(AutrePrenomValide));
	}

	[Test]
	public void SetPrenom_WhenGivenValidPrenomThatHasSpacesAround_ShouldSetPrenomToGivenTrimmedPrenom()
	{
		// Act
		Entite.SetPrenom($" {AutrePrenomValide} ");

		// Assert
		Assert.That(Entite.Prenom, Is.EqualTo(AutrePrenomValide));
	}

	[Test]
	public override void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{PrenomValide} {NomValide}"));
	}
}