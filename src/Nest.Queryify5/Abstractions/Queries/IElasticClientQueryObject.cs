using System.Threading.Tasks;
using Nest.Queryify5.Exceptions;

namespace Nest.Queryify5.Abstractions.Queries
{
    public interface IElasticClientQueryObject<TResponse> where TResponse : class
    {
        /// <exception cref="ElasticClientQueryObjectException">will be thrown on any query error</exception>
        TResponse Execute(IElasticClient client, string index = null);

        /// <returns></returns>        
        /// <exception cref="ElasticClientQueryObjectException">will be thrown on any query error</exception>
        Task<TResponse> ExecuteAsync(IElasticClient client, string index = null);
    }
}