using ERDM.Core;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Interfaces;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ERDM.Credit.Infrastructure.Repositories
{
    public class CreditApplicationRepository : MongoRepository<CreditApplication>, ICreditApplicationRepository
    {
        public CreditApplicationRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<CreditApplicationRepository> logger) : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<CreditApplication>>
            {
                new CreateIndexModel<CreditApplication>(
                    Builders<CreditApplication>.IndexKeys.Ascending(x => x.ApplicationId),
                    new CreateIndexOptions { Unique = true, Name = "idx_application_id" }),

                new CreateIndexModel<CreditApplication>(
                    Builders<CreditApplication>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),

                new CreateIndexModel<CreditApplication>(
                    Builders<CreditApplication>.IndexKeys.Combine(
                        Builders<CreditApplication>.IndexKeys.Ascending(x => x.Status),
                        Builders<CreditApplication>.IndexKeys.Descending(x => x.CreatedAt)),
                    new CreateIndexOptions { Name = "idx_status_created_at" }),

                new CreateIndexModel<CreditApplication>(
                    Builders<CreditApplication>.IndexKeys.Ascending(x => x.ExpiresAt),
                    new CreateIndexOptions {
                        Name = "idx_expires_at_ttl",
                        ExpireAfter = TimeSpan.Zero
                    })
            };

            foreach (var index in indexModels)
            {
                try
                {
                    _collection.Indexes.CreateOne(index);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error creating index {IndexName}. It might already exist.", index.Options.Name);
                }
            }
        }

        public async Task<CreditApplication> GetByApplicationIdAsync(string applicationId)
        {
            var filter = Builders<CreditApplication>.Filter.Eq(x => x.ApplicationId, applicationId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CreditApplication>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<CreditApplication>.Filter.Eq(x => x.CustomerId, customerId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<CreditApplication>> GetByStatusAsync(string status)
        {
            var filter = Builders<CreditApplication>.Filter.Eq(x => x.Status, status);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<CreditApplication>> GetPendingApplicationsAsync()
        {
            var filter = Builders<CreditApplication>.Filter.In(x => x.Status,
                new[] { "PENDING", "SUBMITTED", "UNDERWRITING" });
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<decimal> GetTotalApprovedAmountByCustomerAsync(string customerId)
        {
            var filter = Builders<CreditApplication>.Filter.And(
                Builders<CreditApplication>.Filter.Eq(x => x.CustomerId, customerId),
                Builders<CreditApplication>.Filter.Eq(x => x.Status, "APPROVED"));

            var result = await _collection.Aggregate()
                .Match(filter)
                .Group(x => x.CustomerId, g => new { Total = g.Sum(x => x.Decision.ApprovedAmount ?? 0) })
                .FirstOrDefaultAsync();

            return result?.Total ?? 0;
        }

        public async Task<Dictionary<string, int>> GetApplicationStatisticsByStatusAsync()
        {
            var result = await _collection.Aggregate()
                .Group(x => x.Status, g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return result.ToDictionary(x => x.Status, x => x.Count);
        }

        public async Task<PaginatedResult<CreditApplication>> GetPaginatedByCustomerAsync(string customerId, int pageNumber, int pageSize, string sortBy = "CreatedAt", bool sortDescending = true)
        {
            // Create the filter predicate
            Expression<Func<CreditApplication, bool>> predicate = x => x.CustomerId == customerId;

            // Build sort expression based on string parameter
            Expression<Func<CreditApplication, object>> sortExpression = sortBy?.ToLower() switch
            {
                "amount" => x => x.RequestedAmount,
                "status" => x => x.Status,
                "applicationid" => x => x.ApplicationId,
                "createdat" => x => x.CreatedAt,
                _ => x => x.CreatedAt
            };

            // Pass parameters in the correct order:
            // pageNumber, pageSize, predicate, sortBy, sortDescending
            return await GetPaginatedAsync(pageNumber, pageSize, predicate, sortExpression, sortDescending);
        }
    }
}