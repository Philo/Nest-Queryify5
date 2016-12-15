using System.Threading.Tasks;
using Nest.Queryify.Abstractions.Queries;

namespace Nest.Queryify.Queries
{
	public class DocumentExistsByIdQuery<T> : ElasticClientQueryObject<IExistsResponse> where T : class
	{
		private readonly DocumentPath<T> _documentPath;

		public DocumentExistsByIdQuery(Id id)
		{
            _documentPath = DocumentPath<T>.Id(id);
		}

		protected override IExistsResponse ExecuteCore(IElasticClient client, string index)
		{
			return client.DocumentExists(_documentPath, desc => BuildQueryCore(desc).Index(index));
		}

	    protected override async Task<IExistsResponse> ExecuteCoreAsync(IElasticClient client, string index)
	    {
            return await client.DocumentExistsAsync(_documentPath, desc => BuildQueryCore(desc).Index(index)).ConfigureAwait(false);
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