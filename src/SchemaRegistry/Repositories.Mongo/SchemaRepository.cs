using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MarshmallowPie.Repositories.Mongo
{
    public class SchemaRepository
        : ISchemaRepository
    {
        private readonly IMongoCollection<Schema> _schemas;
        private readonly IMongoCollection<SchemaVersion> _schemaVersions;

        public SchemaRepository(
            IMongoCollection<Schema> schemas,
            IMongoCollection<SchemaVersion> schemaVersions)
        {
            _schemas = schemas;
            _schemaVersions = schemaVersions;
        }

        public IQueryable<Schema> GetSchemas() => _schemas.AsQueryable();

        public Task<Schema> GetSchemaAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return _schemas.AsQueryable()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyDictionary<Guid, Schema>> GetSchemasAsync(
            IReadOnlyList<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            var list = new List<Guid>(ids);

            List<Schema> result = await _schemas.AsQueryable()
                .Where(t => list.Contains(t.Id))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return result.ToDictionary(t => t.Id);
        }


        public Task AddSchemaAsync(
            Schema schema,
            CancellationToken cancellationToken = default)
        {
            return _schemas.InsertOneAsync(
                schema,
                options: null,
                cancellationToken);
        }

        public Task UpdateSchemaAsync(
            Schema schema,
            CancellationToken cancellationToken = default)
        {
            return _schemas.ReplaceOneAsync(
                Builders<Schema>.Filter.Eq(t => t.Id, schema.Id),
                schema,
                options: default(ReplaceOptions),
                cancellationToken);
        }

        public Task AddSchemaVersionAsync(SchemaVersion schema, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SchemaVersion> GetSchemaVersions()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<Guid, SchemaVersion>> GetSchemaVersionsAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSchemaVersionAsync(SchemaVersion schema, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
