using System.Reflection;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Repositories;
using CineQuebec.Domain.Interfaces.Entities.Films;

using Moq;

namespace Tests.Application.Services;

public abstract class GenericServiceTests<TService> where TService : class
{
    private IUnitOfWorkFactory _unitOfWorkFactory = null!;
    private IUnitOfWork _unitOfWork = null!;
    private IRepository<IFilm> _filmRepository = null!;
    private IRepository<IActeur> _acteurRepository = null!;
    private IRepository<IRealisateur> _realisateurRepository = null!;
    private IRepository<ICategorieFilm> _categorieFilmRepository = null!;

    protected TService Service { get; private set; } = null!;
    private Mock<IUnitOfWorkFactory> UnitOfWorkFactoryMock { get; set; } = null!;
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; } = null!;
    protected Mock<IRepository<IFilm>> FilmRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IActeur>> ActeurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<IRealisateur>> RealisateurRepositoryMock { get; private set; } = null!;
    protected Mock<IRepository<ICategorieFilm>> CategorieFilmRepositoryMock { get; private set; } = null!;

    private static TService CreateInstance(params object?[] args)
    {
        try
        {
            return (TService)Activator.CreateInstance(typeof(TService), args)!;
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException!;
        }
    }

    [SetUp]
    public void SetUp()
    {
        UnitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        FilmRepositoryMock = new Mock<IRepository<IFilm>>();
        ActeurRepositoryMock = new Mock<IRepository<IActeur>>();
        RealisateurRepositoryMock = new Mock<IRepository<IRealisateur>>();
        CategorieFilmRepositoryMock = new Mock<IRepository<ICategorieFilm>>();

        _unitOfWorkFactory = UnitOfWorkFactoryMock.Object;
        _unitOfWork = UnitOfWorkMock.Object;
        _filmRepository = FilmRepositoryMock.Object;
        _acteurRepository = ActeurRepositoryMock.Object;
        _realisateurRepository = RealisateurRepositoryMock.Object;
        _categorieFilmRepository = CategorieFilmRepositoryMock.Object;

        UnitOfWorkMock.Setup(u => u.FilmRepository).Returns(_filmRepository);
        UnitOfWorkMock.Setup(u => u.ActeurRepository).Returns(_acteurRepository);
        UnitOfWorkMock.Setup(u => u.RealisateurRepository).Returns(_realisateurRepository);
        UnitOfWorkMock.Setup(u => u.CategorieFilmRepository).Returns(_categorieFilmRepository);
        UnitOfWorkFactoryMock.Setup(f => f.Create()).Returns(_unitOfWork);

        SetUpExt();

        Service = CreateInstance(_unitOfWorkFactory);
    }

    protected virtual void SetUpExt() { }
}