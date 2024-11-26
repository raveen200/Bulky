namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWorks
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        void Save();


    }
}
