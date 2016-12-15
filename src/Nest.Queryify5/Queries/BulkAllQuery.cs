using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest.Queryify.Abstractions.Queries;

namespace Nest.Queryify.Queries
{
    public class BulkAllQuery<T> : ElasticClientQueryObject<BulkAllObservable<T>> where T : class
    {
        private readonly IEnumerable<T> _documents;
        private readonly Refresh _refreshOnSave;

        public BulkAllQuery(IEnumerable<T> documents, bool refreshOnSave = false) : this(documents, refreshOnSave ? Refresh.True : Refresh.False)
        {
        }

        public BulkAllQuery(IEnumerable<T> documents, Refresh refreshOnSave = Refresh.False)
        {
            _documents = documents;
            _refreshOnSave = refreshOnSave;
        }

        protected override BulkAllObservable<T> ExecuteCore(IElasticClient client, string index)
        {
            return client.BulkAll(_documents, desc => BuildQueryCore(desc, index, _refreshOnSave));
        }

        protected override Task<BulkAllObservable<T>> ExecuteCoreAsync(IElasticClient client, string index)
        {
            throw new NotSupportedException("bulk all does not have a direct async method");
        }

        protected virtual BulkAllDescriptor<T> BuildQueryCore(BulkAllDescriptor<T> descriptor,
            string index, Refresh refreshOnSave)
        {
            return BuildQuery(descriptor).Refresh(refreshOnSave);
        }

        protected virtual BulkAllDescriptor<T> BuildQuery(BulkAllDescriptor<T> descriptor)
        {
            return descriptor;
        }
    }
}