using System.Threading.Tasks;
using Nest.Queryify5.Abstractions.Queries;

namespace Nest.Queryify5.Queries
{
    public abstract class MultiSearchQuery : ElasticClientQueryObject<IMultiSearchResponse>
    {
        protected override IMultiSearchResponse ExecuteCore(IElasticClient client, string index)
        {
			return client.MultiSearch(desc => BuildQueryCore(desc, index));
        }

        protected override async Task<IMultiSearchResponse> ExecuteCoreAsync(IElasticClient client, string index)
        {
            return await client.MultiSearchAsync(desc => BuildQueryCore(desc, index)).ConfigureAwait(false);
        }

        protected MultiSearchDescriptor BuildQueryCore(MultiSearchDescriptor descriptor, string index)
        {
            descriptor = BuildQuery(descriptor, index);
            return descriptor;
        }

        protected abstract MultiSearchDescriptor BuildQuery(MultiSearchDescriptor descriptor, string index);
    }
}