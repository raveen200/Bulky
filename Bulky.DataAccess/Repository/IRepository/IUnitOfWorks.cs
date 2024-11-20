namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWorks
    {
        ICategoryRepository Category { get; }
        void Save();
    }
}
