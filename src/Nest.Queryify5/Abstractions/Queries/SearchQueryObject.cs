namespace Nest.Queryify5.Abstractions.Queries
{
	public abstract class SearchQueryObject<TDocument> : ElasticClientQueryObject<ISearchResponse<TDocument>> where TDocument : class
	{
	}
}