using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest.Queryify.Exceptions;

namespace Nest.Queryify.Abstractions.Queries
{
    [DebuggerStepThrough]
    public abstract class ElasticClientQueryObject<TResponse> : IElasticClientQueryObject<TResponse> where TResponse : class
    {
	    public TResponse Execute(IElasticClient client, string index)
	    {
	        return WrapQueryResponse(() => ExecuteCore(client, index));
	    }

        public Task<TResponse> ExecuteAsync(IElasticClient client, string index)
        {
            return WrapQueryResponse(() => ExecuteCoreAsync(client, index));
        }

        protected abstract TResponse ExecuteCore(IElasticClient client, string index);

        protected abstract Task<TResponse> ExecuteCoreAsync(IElasticClient client, string index);

        private static TQueryResponse WrapQueryResponse<TQueryResponse>(Func<TQueryResponse> execute) where TQueryResponse : class
        {
            try
            {
                return execute();
            }
            catch (ElasticClientQueryObjectException)
            {
                throw;
            }
            catch (ElasticsearchClientException exception)
            {
                throw new ElasticClientQueryObjectException("There was an error executing the query", exception);
            }
            catch (Exception exception)
            {
                throw new ElasticClientQueryObjectException($"An unexpected query execution error occurred: {exception.Message}", exception);
            }
        }
    }
}