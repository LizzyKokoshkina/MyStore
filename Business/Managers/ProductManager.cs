using Business.Interfaces;
using Core.Database.UnitOfWork;
using Core.DTO;
using Core.Entites;
using Core.Enums;

namespace Business.Managers
{
    public class ProductManager : ManagerBase, IProductManager
    {
        public ProductManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task<IEnumerable<ProductDto>> Get(CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Product>().Get(x => true, x => new ProductDto
            {
                Id = x.Id,
                Description = x.Description,
                Title = x.Title,
                Sizes = x.Sizes,
                IsOnSale = x.IsOnSale,
                Color = x.Color,
                Price = x.Price,
                Quantity = x.Quantity,
                Categories = x.Categories.Select(c => new Category
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name
                }).ToList(),
            }, cancellationToken);
        }

        public Task<CategoryDto> GetByCategory(int categoryId, CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Category>().GetFirst(x => x.Id == categoryId, c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(x => new ProductDto
                {
                    Id = x.Product.Id,
                    Description = x.Product.Description,
                    Title = x.Product.Title,
                    Sizes = x.Product.Sizes,
                    IsOnSale = x.Product.IsOnSale,
                    Color = x.Product.Color,
                    Price = x.Product.Price,
                    Quantity = x.Product.Quantity,
                })
            });
        }

        public Task<ProductDto> GetProduct(int productId, CancellationToken cancellationToken = default)
        {
            return UnitOfWork.Sql<Product>().GetFirst(x => x.Id == productId, x => new ProductDto
            {
                Id = x.Id,
                Description = x.Description,
                Title = x.Title,
                Sizes = x.Sizes,
                IsOnSale = x.IsOnSale,
                Color = x.Color,
                Price = x.Price,
                Quantity = x.Quantity,
            }, cancellationToken);
        }

        public async Task<Product> Create(Product product, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Product>().Create(product, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return entity;
        }

        public async Task<Result> Update(Product product, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<Product>().GetFirst(x => x.Id == product.Id, x => x, cancellationToken);
            if (entity == null)
            {
                return Result.Fail;
            }
            entity.Description = product.Description;
            entity.Title = product.Title;
            entity.Sizes = product.Sizes;
            entity.IsOnSale = product.IsOnSale;
            entity.Color = product.Color;
            entity.Price = product.Price;
            entity.Quantity = product.Quantity;
            await UnitOfWork.Sql<Product>().Update(entity, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }

        public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
        {
            await UnitOfWork.Sql<Product>().Delete(x => x.Id == id, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }
    }
}
