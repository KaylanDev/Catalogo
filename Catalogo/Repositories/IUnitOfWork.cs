namespace Catalogo.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task  Commit();
    }
}
