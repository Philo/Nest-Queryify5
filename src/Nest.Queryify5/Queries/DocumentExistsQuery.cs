using System.Threading.Tasks;
using Nest.Queryify5.Abstractions.Queries;

namespace Nest.Queryify5.Queries
{
	public class DocumentExistsQuery<T> : ElasticClientQueryObject<IExistsResponse> where T : class
	{
		private readonly T _document;

		public DocumentExistsQuery(T document)
		{
			_document = document;
		}

		protected override IExistsResponse ExecuteCore(IElasticClient client, string index)
		{
			return client.DocumentExists(DocumentPath<T>.Id(_document), desc => BuildQueryCore(desc).Index(index));
		}

	    protected override async Task<IExistsResponse> ExecuteCoreAsync(IElasticClient client, string index)
	    {
            return await client.DocumentExistsAsync(DocumentPath<T>.Id(_document), desc => BuildQueryCore(desc).Index(index)).ConfigureAwait(false);
        }

	    protected virtual DocumentExistsDescriptor<T> BuildQueryCore(DocumentExistsDescriptor<T> descriptor)
		{
			return BuildQuery(descriptor);
		}

		protected virtual DocumentExistsDescriptor<T> BuildQuery(DocumentExistsDescriptor<T> descriptor)
		{
			return descriptor;
		}
	}
}