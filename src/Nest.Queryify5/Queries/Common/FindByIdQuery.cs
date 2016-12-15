using System.Threading.Tasks;

namespace Nest.Queryify.Queries.Common
{
    public class FindByIdQuery<T> : DocumentPathQuery<T, T> where T : class
    {
        public FindByIdQuery(Id id) : base(id)
        {
            
        }

        protected override T ExecuteCore(IElasticClient client, string index, DocumentPath<T> documentPath)
        {
            return client.Get(documentPath, desc => desc.Index(index))?.Source;
        }

        protected override async Task<T> ExecuteCoreAsync(IElasticClient client, string index, DocumentPath<T> documentPath)
        {
            var task = await client.GetAsync(documentPath, desc => desc.Index(index)).ConfigureAwait(false);
            return task.Source;
        }
    }
}