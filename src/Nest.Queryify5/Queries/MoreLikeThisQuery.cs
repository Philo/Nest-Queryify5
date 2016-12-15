using System.Threading.Tasks;
using Nest.Queryify.Abstractions.Queries;

namespace Nest.Queryify.Queries
{
    public abstract class MoreLikeThisQuery<T> : SearchQueryObject<T>
		where T : class
	{
		protected override ISearchResponse<T> ExecuteCore(IElasticClient client, string index)
		{
		    return client.Search<T>(s => s.Query(q => q.MoreLikeThis(BuildQuery)).Index(index));
		}

	    protected override async Task<ISearchResponse<T>> ExecuteCoreAsync(IElasticClient client, string index)
	    {
            return await client.SearchAsync<T>(s => s.Query(q => q.MoreLikeThis(BuildQuery)).Index(index)).ConfigureAwait(false);
        }

	    protected abstract MoreLikeThisQueryDescriptor<T> BuildQuery(MoreLikeThisQueryDescriptor<T> descriptor);
	}
}