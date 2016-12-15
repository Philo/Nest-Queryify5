using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest.Queryify5.Abstractions.Queries;

namespace Nest.Queryify5.Queries
{
	public class BulkIndexDocumentQuery<T> : ElasticClientQueryObject<IBulkResponse> where T : class
	{
		private readonly IEnumerable<T> _documents;
		private readonly Refresh _refreshOnSave;

		public BulkIndexDocumentQuery(IEnumerable<T> documents, bool refreshOnSave = false) : this(documents, refreshOnSave ? Refresh.True : Refresh.False)
		{
		}

        public BulkIndexDocumentQuery(IEnumerable<T> documents, Refresh refreshOnSave = Refresh.False)
        {
            _documents = documents;
            _refreshOnSave = refreshOnSave;
        }

        protected override IBulkResponse ExecuteCore(IElasticClient client, string index)
		{
			return client.Bulk(desc => BuildQueryCore(desc, index, _refreshOnSave));
		}

	    protected override async Task<IBulkResponse> ExecuteCoreAsync(IElasticClient client, string index)
	    {
            return await client.BulkAsync(desc => BuildQueryCore(desc, index, _refreshOnSave)).ConfigureAwait(false);
        }

	    protected virtual BulkDescriptor BuildQueryCore(BulkDescriptor descriptor,
			string index, Refresh refreshOnSave)
		{
			descriptor = descriptor
				.IndexMany(_documents, (d, i) => d
                    .Type(i.GetType())
					.Index(index)
				)
				.Refresh(refreshOnSave);
			return BuildQuery(descriptor);
		}

		protected virtual BulkDescriptor BuildQuery(BulkDescriptor descriptor)
		{
			return descriptor;
		}
	}
}