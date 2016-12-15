using System.Threading.Tasks;

namespace Nest.Queryify.Abstractions.Queries
{
    public abstract class ScrollSearchQueryObject<TDocument> : SearchQueryObject<TDocument> where TDocument : class
    {
        private readonly Time _time;
        private readonly string _scrollId;

        protected ScrollSearchQueryObject(Time time, string scrollId)
        {
            _time = time;
            _scrollId = scrollId;
        }

        protected override ISearchResponse<TDocument> ExecuteCore(IElasticClient client, string index)
        {
            return client.Scroll<TDocument>(_time, _scrollId, desc => ModifyDescriptor(desc).Scroll(_time).ScrollId(_scrollId));
        }

        protected virtual ScrollDescriptor<TDocument> ModifyDescriptor(ScrollDescriptor<TDocument> desc)
        {
            return desc;
        }

        protected override async Task<ISearchResponse<TDocument>> ExecuteCoreAsync(IElasticClient client, string index)
        {
            return await client.ScrollAsync<TDocument>(_time, _scrollId, desc => ModifyDescriptor(desc).Scroll(_time).ScrollId(_scrollId)).ConfigureAwait(false);
        }
    }
}