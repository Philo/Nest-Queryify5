using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest.Queryify5.Queries.Common
{
    public class DeleteByIdQuery<T> : DocumentPathQuery<T, IDeleteResponse> where T : class
    {
        private readonly Id _id;
        private readonly Refresh _refreshOnDelete;

        public DeleteByIdQuery(Id id, bool refreshOnDelete = false) : this(id, refreshOnDelete ? Refresh.True : Refresh.False)
        {
        }

        public DeleteByIdQuery(Id id, Refresh refreshOnDelete = Refresh.False) : base(id)
        {
            _id = id;
            _refreshOnDelete = refreshOnDelete;
        }

        protected override IDeleteResponse ExecuteCore(IElasticClient client, string index, DocumentPath<T> documentPath)
        {
            return client.Delete(documentPath, descriptor => BuildQueryCore(descriptor).Index(index));
        }

        protected override async Task<IDeleteResponse> ExecuteCoreAsync(IElasticClient client, string index, DocumentPath<T> documentPath)
        {
            return await client.DeleteAsync(documentPath, descriptor => BuildQueryCore(descriptor).Index(index)).ConfigureAwait(false);
        }

        protected virtual DeleteDescriptor<T> BuildQueryCore(DeleteDescriptor<T> descriptor)
        {
            return BuildQuery(descriptor).Refresh(_refreshOnDelete);
        }

        protected virtual DeleteDescriptor<T> BuildQuery(DeleteDescriptor<T> descriptor)
        {
            return descriptor;
        }
    }
}