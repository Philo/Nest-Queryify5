using System.Threading.Tasks;
using Nest.Queryify5.Abstractions.Queries;

namespace Nest.Queryify5.Queries.Common
{
    public abstract class DocumentPathQuery<T, TResponse> : ElasticClientQueryObject<TResponse> where T : class where TResponse : class
    {
        private readonly DocumentPath<T> _documentPath;
        protected DocumentPathQuery(DocumentPath<T> documentPath)
        {
            _documentPath = documentPath;
        }

        protected DocumentPathQuery(Id id) : this(DocumentPath<T>.Id(id))
        {

        }

        protected DocumentPathQuery(T document) : this(DocumentPath<T>.Id(document))
        {

        }

        protected sealed override async Task<TResponse> ExecuteCoreAsync(IElasticClient client, string index)
        {
            return await ExecuteCoreAsync(client, index, _documentPath).ConfigureAwait(false);
        }

        protected sealed override TResponse ExecuteCore(IElasticClient client, string index)
        {
            return ExecuteCore(client, index, _documentPath);
        }

        protected abstract TResponse ExecuteCore(IElasticClient client, string index, DocumentPath<T> documentPath);

        protected abstract Task<TResponse> ExecuteCoreAsync(IElasticClient client, string index, DocumentPath<T> documentPath);
    }
}