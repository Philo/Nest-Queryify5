using System.Threading.Tasks;
using Nest.Queryify5.Abstractions.Queries;

namespace Nest.Queryify5.Extensions
{
    public static class ElasticClientQueryExtensions
    {
        public static TResponse Query<TResponse>(this IElasticClient client, IElasticClientQueryObject<TResponse> query, string index = null) where TResponse : class
        {
            return query.Execute(client, index);
        }

        public static Task<TResponse> QueryAsync<TResponse>(this IElasticClient client, IElasticClientQueryObject<TResponse> query, string index = null) where TResponse : class
        {
            return query.ExecuteAsync(client, index);
        }
    }
}
