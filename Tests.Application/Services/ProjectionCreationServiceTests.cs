﻿using System.Linq.Expressions;

using CineQuebec.Application.Services;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

using Moq;

namespace Tests.Application.Services;

public class ProjectionCreationServiceTests : GenericServiceTests<ProjectionCreationService>
{
    private static readonly Guid IdFilmValide = Guid.NewGuid();
    private static readonly Guid IdSalleValide = Guid.NewGuid();
    private static readonly DateTime DateValide = DateTime.Now + TimeSpan.FromDays(1);
    private static readonly DateTime DateDansLePasse = DateTime.Now - TimeSpan.FromDays(1);


    [Test]
    public void
        CreerProjection_WhenGivenIdFilmAndIdSalleAndDateHeureAlreadyPrensentInRepository_ThrowsAggregateExceptionContainingArgumentException()
    {
        //Arrange
        ProjectionRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IProjection, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerProjection(IdFilmValide, IdSalleValide, DateValide, false));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("existe déjà"));
    }


    [Test]
    public void CreerProjection_WhenGivenInvalidDate_ThrowsAggregateExceptionContainingArgumentException()
    {
        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerProjection(IdFilmValide, IdSalleValide, DateDansLePasse, false));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message
                .Contains("date de projection ne doit pas être dans le passé"));
    }


    [Test]
    public void CreerProjection_WhenGivenInvalidFilm_ThrowsAggregateExceptionContainingArgumentException()
    {
        //Arrange
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerProjection(IdFilmValide, IdSalleValide, DateValide, false));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }


    [Test]
    public void CreerProjection_WhenGivenInvalidSalle_ThrowsAggregateExceptionContainingArgumentException()
    {
        //Arrange
        SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdSalleValide))
            .ReturnsAsync((Salle?)null);

        // Act & Assert
        AggregateException? aggregateException = Assert.ThrowsAsync<AggregateException>(() =>
            Service.CreerProjection(IdFilmValide, IdSalleValide, DateValide, false));
        Assert.That(aggregateException?.InnerExceptions,
            Has.One.InstanceOf<ArgumentException>().With.Message.Contains("n'existe pas"));
    }

    [Test]
    public async Task CreerProjection_WhenGivenValidArguments_ShouldAddProjectionToRepository()
    {
        //Arrange
        ProjectionRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IProjection, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));
        SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdSalleValide))
            .ReturnsAsync(Mock.Of<ISalle>(cf => cf.Id == IdSalleValide));
        ProjectionRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Projection>()))
            .ReturnsAsync(Mock.Of<IProjection>(a => a.Id == Guid.NewGuid()));

        // Act
        await Service.CreerProjection(IdFilmValide, IdSalleValide, DateValide, false);

        // Assert
        ProjectionRepositoryMock.Verify(r => r.AjouterAsync(It.IsAny<Projection>()));
    }

    [Test]
    public async Task CreerProjection_WhenGivenValidArguments_ShouldReturnGuidOfNewProjection()
    {
        //Arrange
        ProjectionRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<IProjection, bool>>>()))
            .ReturnsAsync(false);
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));
        SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdSalleValide))
            .ReturnsAsync(Mock.Of<ISalle>(cf => cf.Id == IdSalleValide));
        ProjectionRepositoryMock.Setup(r => r.AjouterAsync(It.IsAny<Projection>()))
            .ReturnsAsync(Mock.Of<IProjection>(a => a.Id == Guid.NewGuid()));

        // Act
        Guid projectionId = await Service.CreerProjection(IdFilmValide, IdSalleValide, DateValide, false);

        // Assert
        Assert.That(projectionId, Is.Not.EqualTo(Guid.Empty));
    }

    protected override void SetUpExt()
    {
        FilmRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdFilmValide))
            .ReturnsAsync(Mock.Of<IFilm>(cf => cf.Id == IdFilmValide));

        SalleRepositoryMock.Setup(r => r.ObtenirParIdAsync(IdSalleValide))
            .ReturnsAsync(Mock.Of<ISalle>(cf => cf.Id == IdSalleValide));
    }
}