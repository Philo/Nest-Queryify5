using System.Threading.Tasks;
using Nest.Queryify.Abstractions.Queries;

namespace Nest.Queryify.Queries.Common
{
    public class GetByIdQuery<T> : ElasticClientQueryObject<IGetResponse<T>> where T : class
    {
        private readonly DocumentPath<T> _documentPath;
        protected GetByIdQuery(DocumentPath<T> documentPath)
        {
            _documentPath = documentPath;
        }

        public GetByIdQuery(Id id) : this(DocumentPath<T>.Id(id))
        {
            
        }

        public GetByIdQuery(T document) : this(DocumentPath<T>.Id(document))
        {
            
        }

        protected override IGetResponse<T> ExecuteCore(IElasticClient client, string index)
        {
            return client.Get(_documentPath, desc => BuildQuery(desc).Index(index));
        }

        protected override async Task<IGetResponse<T>> ExecuteCoreAsync(IElasticClient client, string index)
        {
            return await client.GetAsync(_documentPath, desc => BuildQuery(desc).Index(index)).ConfigureAwait(false);
        }

        protected virtual GetDescriptor<T> BuildQuery(GetDescriptor<T> descriptor)
        {
            return descriptor;
        }
    }
}