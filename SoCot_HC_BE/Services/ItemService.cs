using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System.Threading;

namespace SoCot_HC_BE.Services
{
    public class ItemService : Repository<Item, Guid>, IItemService
    {

        public ItemService(AppDbContext context) : base(context)
        {

        }
        public override async Task<Item?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = await _dbSet
                  .Include(e => e.ItemCategory)
                  .Include(e => e.SubCategory)
                  .Include(e => e.Product)
                  .Include(e => e.Form)
                  .Include(e => e.Strength)
                  .Include(e => e.Route)
                  //.Include(e => e.UoM)
                  .FirstOrDefaultAsync(f => f.ItemId == id, cancellationToken);

            return item;
        }

        public async Task<PaginationHandler<Item>> GetAllWithPagingAsync(int pageNo, int statusId, List<Guid>? itemCategories, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {

            int totalRecords = await _dbSet.CountAsync(d =>
                                    (statusId == 0 || (statusId == 1 && d.IsActive) || (statusId == 2 && !d.IsActive)) &&
                                    (itemCategories == null || !itemCategories.Any() || itemCategories.Contains(d.ItemCategoryId)) &&
                                    (string.IsNullOrEmpty(keyword) || d.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                );

            var items = await _dbSet
                .Include(i => i.ItemCategory)
                        .Include(i => i.Product)
                            .Where(d =>
                                (statusId == 0 || (statusId == 1 && d.IsActive) || (statusId == 2 && !d.IsActive)) &&
                                 (itemCategories == null || !itemCategories.Any() || itemCategories.Contains(d.ItemCategoryId)) &&
                                (string.IsNullOrEmpty(keyword) || d.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                            )
                            .AsNoTracking()
                            .ToListAsync();

            var paginatedResult = new PaginationHandler<Item>(items, totalRecords, pageNo, limit);
            return paginatedResult;
        }


        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                        .Where(v => v.Description.Contains(keyword));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }



        public async Task SaveItemAsync(ItemDTO itemDTO, CancellationToken cancellationToken = default)
        {
            bool isNew = itemDTO.ItemId == Guid.Empty;

            ValidateFields(itemDTO);
            var item = new Item
            {
                ItemId = itemDTO.ItemId,
                ItemCategoryId = itemDTO.ItemCategoryId,
                ProductId = itemDTO.ProductId,
                Code = itemDTO.Code,
                Description = itemDTO.Description,
                BrandName = itemDTO.BrandName,

                SubCategoryId = itemDTO.SubCategoryId,
                FormId = itemDTO.FormId,
                StrengthId =  itemDTO.StrengthId ,
                StrengthNo = itemDTO.StrengthNo,
                RouteId = itemDTO.RouteId ,
            };


            if (isNew) {
                item.ItemId = Guid.NewGuid();
                item.IsActive = true;
                await AddAsync(item, cancellationToken);
            }
            else
            {

                var existing = await _dbSet.FindAsync(new object[] { item.ItemCategoryId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Item not found.");

                _context.Entry(existing).CurrentValues.SetValues(item);

                await UpdateAsync(existing, cancellationToken);

            }

        }

        private void ValidateFields(ItemDTO item)
        {

            var errors = new Dictionary<string, List<string>>();
            var itemCategory = _context.Set<ItemCategory>()
                             .Find(item.ItemCategoryId);



            if (itemCategory == null)
            {
                ValidationHelper.IsRequired(errors, nameof(item.ItemCategoryId), item.ItemCategoryId, "Item Category");
                ValidationHelper.IsRequired(errors, nameof(item.FormId), item.FormId, "Form");
                ValidationHelper.IsRequired(errors, nameof(item.StrengthId), item.StrengthId, "Strength");
                ValidationHelper.IsRequired(errors, nameof(item.StrengthNo), item.StrengthNo, "Strength No");
                ValidationHelper.IsRequired(errors, nameof(item.RouteId), item.RouteId, "Route");
                ValidationHelper.IsRequired(errors, nameof(item.SubCategoryId), item.SubCategoryId, "Sub Category");

            }
            else {
                var categoryDesc = itemCategory.Description;
                if (categoryDesc.Equals("Drugs and Medicine"))
                {
                    ValidationHelper.IsRequired(errors, nameof(item.FormId), item.FormId, "Form");
                    ValidationHelper.IsRequired(errors, nameof(item.StrengthId), item.StrengthId, "Strength");
                    ValidationHelper.IsRequired(errors, nameof(item.StrengthNo), item.StrengthNo, "Strength No");
                    ValidationHelper.IsRequired(errors, nameof(item.RouteId), item.RouteId, "Route");
                }
                else if (categoryDesc.Equals("Laboratory Supply"))
                {
                    ValidationHelper.IsRequired(errors, nameof(item.SubCategoryId), item.SubCategoryId, "Sub Category");
                }

            }

            ValidationHelper.IsRequired(errors, nameof(item.ProductId), item.ProductId, "Product");
            ValidationHelper.IsRequired(errors, nameof(item.Description), item.Description, "Description");


                if (errors.Any())
                    throw new ModelValidationException("Validation failed", errors);

        }

    }
}
