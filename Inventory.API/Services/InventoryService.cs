using AutoMapper;
using Infrastructure.Common.Repository;
using Inventory.API.Entities;
using Inventory.API.Extensions;
using Inventory.API.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations.Database;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.API.Services
{
    public class InventoryService : MongoDBRepository<InventoryEntry>, IIventoryService
    {
        private IMapper _mapper;
        public InventoryService(IMongoClient mongoClient, MongoSettings settings, IMapper mapper) : base(mongoClient, settings)
        {
            _mapper = mapper;
        }

        public async Task DeleteByDocumentNoAsync(string documentNo)
        {
            FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(field: s => s.DocumentNo, documentNo);
            await Collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo)
        {
            var filter = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, itemNo);
            var entries = await FindAll().Find(filter).ToListAsync();
            return _mapper.Map<IEnumerable<InventoryEntryDto>>(entries);
        }

        public async Task<InventoryEntryDto> GetByIdAsync(string id)
        {
            var filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);

            var entry = await FindAll()
                .Find(filter)
                .FirstOrDefaultAsync(); 

            if (entry == null)
            {
                throw new Exception("Inventory entry not found");
            }
            return _mapper.Map<InventoryEntryDto>(entry);
        }

        public async Task<PagedList<InventoryEntryDto>> GetPageByItemNoAsync(GetInventoryPagingQuery query)
        {
            var builder = Builders<InventoryEntry>.Filter;
            var filters = new List<FilterDefinition<InventoryEntry>>();
            filters.Add(builder.Eq(x => x.ItemNo, query.ItemNo));

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                filters.Add(builder.Regex(x => x.ItemNo, new MongoDB.Bson.BsonRegularExpression($".*{query.SearchTerm}.*", "i")));
            }

            var filter = builder.And(filters);

            // Get total count for pagination
            var totalItems = await Collection
                .CountDocumentsAsync(filter);

            var entries = await Collection
                .Find(filter)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Limit(query.PageSize)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            var items = _mapper.Map<IEnumerable<InventoryEntryDto>>(entries);

            return new PagedList<InventoryEntryDto>(
                items,
                totalItems,
                query.PageNumber,
                query.PageSize
            );
        }


        public async Task<InventoryEntryDto> PurchaseProduct(string itemNo, PurchaseProductDto model)
        {
            var entity = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItemNo = model.ItemNo,
                Quantity = model.Quantity,
                DocumentType = model.EDocumentType,
            };

            await CreateAsync(entity);
            var result = _mapper.Map<InventoryEntryDto>(entity);

            return result;
        }

        public async Task<string> SaleOrderAsync(SalesOrderDto model)
        {
            var documentNo = Guid.NewGuid().ToString();

            foreach (var saleItem in model.SaleItems)
            {
                var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
                {
                    DocumentNo = documentNo,
                    ItemNo = saleItem.No,
                    ExternalDocumentNo = model.OrderNo,
                    Quantity = saleItem.Quantity * -1,
                    DocumentType = saleItem.DocumentType
                };

                await CreateAsync(itemToAdd);
            }

            return documentNo;
        }

        public async Task<InventoryEntryDto> SalesItemAsync(string itemNo, SalesProductDto model)
        {
            var itemToAdd = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItemNo = itemNo,
                ExternalDocumentNo = model.ExternalDocumentNo,
                Quantity = model.Quantity * -1,
                DocumentType = model.DocumentType
            };

            var entity = _mapper.Map<InventoryEntry>(itemToAdd);
            await CreateAsync(entity);
            var result = _mapper.Map<InventoryEntryDto>(entity);

            return result;
        }

        
    }
}
