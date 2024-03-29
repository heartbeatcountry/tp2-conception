using System.Reflection;
using CineQuebec.Domain.Entities.Abstract;

// ReSharper disable EqualExpressionComparison

namespace Tests.Domain.Entities.Abstract;

public abstract class EntiteTests<TEntite> where TEntite : Entite
{
	protected virtual TEntite Entite { get; set; } = null!;
	protected virtual object?[] ArgsConstructeur => [];

	private class EntiteImpl : Entite;

	[Test]
	public void Constructor_Always_ShouldCreateEntityOfTypeT()
	{
		Assert.That(Entite, Is.InstanceOf<TEntite>());
	}

	[Test]
	public void Constructor_Always_ShouldSetIdToEmptyGuid()
	{
		// Assert
		Assert.That(Entite.Id, Is.EqualTo(Guid.Empty));
	}

	protected TEntite CreateInstance(params object?[] args)
	{
		try
		{
			return (TEntite)Activator.CreateInstance(typeof(TEntite), args)!;
		}
		catch (TargetInvocationException e)
		{
			throw e.InnerException!;
		}
	}

	protected TEntite CreateInstance()
	{
		return CreateInstance(ArgsConstructeur);
	}

	[Test]
	public void Equals_WhenComparingToBoxedNull_ShouldReturnFalse()
	{
		// Assert
		Assert.That(Entite.Equals((object?)null), Is.False);
	}

	[Test]
	public void Equals_WhenComparingToBoxedSameInstance_ShouldReturnTrue()
	{
		// Assert
		Assert.That(Entite.Equals((object)Entite), Is.True);
	}

	[Test]
	public void Equals_WhenComparingToDifferentInstanceWithDifferentId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite.Equals(autre), Is.False);
	}

	[Test]
	public void Equals_WhenComparingToDifferentInstanceWithSameId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite.Equals(autre), Is.True);
	}

	[Test]
	public void Equals_WhenComparingToEntityOfDifferentType_ShouldReturnFalse()
	{
		// Arrange
		var autre = new object();

		// Assert
		Assert.That(Entite.Equals(autre), Is.False);
	}

	[Test]
	public void Equals_WhenComparingToNull_ShouldReturnFalse()
	{
		// Assert
		Assert.That(Entite.Equals(null), Is.False);
	}

	[Test]
	public void Equals_WhenComparingToSameInstance_ShouldReturnTrue()
	{
		// Assert
		Assert.That(Entite.Equals(Entite), Is.True);
	}

	[Test]
	public virtual void GetHashCode_Always_ShouldBeUniqueToTheEntity()
	{
		// Arrange
		var id = Guid.NewGuid();
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.GetHashCode(), Is.EqualTo(id.GetHashCode()));
	}

	[Test]
	public void Instance_WhenAddingToHashSet_ShouldNotAddOtherInstanceWithSameId()
	{
		// Arrange
		var hashSet = new HashSet<TEntite>();
		Entite.SetId(Guid.NewGuid());
		var autre = CreateInstance();
		autre.SetId(Entite.Id);
		hashSet.Add(Entite);

		// Act
		hashSet.Add(autre);

		// Assert
		Assert.That(hashSet, Has.Count.EqualTo(1));
	}

	[Test]
	public void OperatorEqual_WhenComparingToDifferentInstanceWithDifferentId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite == autre, Is.False);
	}

	[Test]
	public void OperatorEqual_WhenComparingToDifferentInstanceWithSameId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite == autre, Is.True);
	}

	[Test]
	public void OperatorEqual_WhenComparingToSameInstance_ShouldReturnTrue()
	{
		// Assert
		Assert.That(Entite == Entite, Is.True);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingDifferentInstanceWithDifferentId_ShouldReturnTrue()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Guid.NewGuid());

		// Assert
		Assert.That(Entite != autre, Is.True);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingDifferentInstanceWithSameId_ShouldReturnFalse()
	{
		// Arrange
		var autre = CreateInstance();
		Entite.SetId(Guid.NewGuid());
		autre.SetId(Entite.Id);

		// Assert
		Assert.That(Entite != autre, Is.False);
	}

	[Test]
	public void OperatorNotEqual_WhenComparingToSameInstance_ShouldReturnFalse()
	{
		// Assert
		Assert.That(Entite != Entite, Is.False);
	}

	[Test]
	public void SetId_WhenGivenEmptyGuid_ShouldThrowArgumentNullException()
	{
		// Assert
		Assert.That(() => Entite.SetId(Guid.Empty), Throws.ArgumentNullException);
	}

	[Test]
	public void SetId_WhenGivenNonEmptyGuid_ShouldSetIdToGivenGuid()
	{
		// Arrange
		var id = Guid.NewGuid();

		// Act
		Entite.SetId(id);

		// Assert
		Assert.That(Entite.Id, Is.EqualTo(id));
	}

	[SetUp]
	public virtual void Setup()
	{
		Entite = CreateInstance();
	}

	[Test]
	public virtual void ToString_Always_ShouldUniquelyDescribeTheEntity()
	{
		// Arrange
		var type = Entite.GetType().Name;
		var id = Entite.Id;

		// Assert
		Assert.That(Entite.ToString(), Is.EqualTo($"{type} {id}"));
	}

	[Test]
	public void ToString_OfDerivedClass_ShouldUniquelyDescribeTheEntity()
	{
		// Arrange
		var entiteImpl = new EntiteImpl();
		var type = entiteImpl.GetType().Name;
		var id = entiteImpl.Id;

		// Assert
		Assert.That(entiteImpl.ToString(), Is.EqualTo($"{type} {id}"));
	}
}