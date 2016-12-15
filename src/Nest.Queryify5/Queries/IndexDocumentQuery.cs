using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest.Queryify.Abstractions.Queries;

namespace Nest.Queryify.Queries
{
	public class IndexDocumentQuery<T> : ElasticClientQueryObject<IIndexResponse> where T : class
	{
		private readonly T _document;
		private readonly Refresh _refreshOnSave;

		public IndexDocumentQuery(T document) : this(document, Refresh.False)
		{
		}

        public IndexDocumentQuery(T document, Refresh refreshOnSave = Refresh.False)
        {
            _document = document;
            _refreshOnSave = refreshOnSave;
        }

        protected override IIndexResponse ExecuteCore(IElasticClient client, string index)
		{
			return client.Index(_document, desc => BuildQueryCore(desc, _refreshOnSave).Index(index));
		}

	    protected override async Task<IIndexResponse> ExecuteCoreAsync(IElasticClient client, string index)
	    {
            return await client.IndexAsync(_document, desc => BuildQueryCore(desc, _refreshOnSave).Index(index)).ConfigureAwait(false);
        }

	    protected virtual IndexDescriptor<T> BuildQueryCore(IndexDescriptor<T> descriptor, Refresh refreshOnSave)
		{
			return BuildQuery(descriptor)
                .Type<T>()
                .Refresh(refreshOnSave);
		}

		protected virtual IndexDescriptor<T> BuildQuery(IndexDescriptor<T> descriptor)
		{
			return descriptor;
		}
	}
}