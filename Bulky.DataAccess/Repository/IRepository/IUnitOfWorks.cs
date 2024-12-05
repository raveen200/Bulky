namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWorks
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetails { get; }

        IApplicationUserRepository ApplicationUser { get; }
        void Save();


    }
}
