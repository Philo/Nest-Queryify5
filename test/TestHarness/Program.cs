using System;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace TestHarness
{
    [ElasticsearchType(IdProperty = "Id", Name = "document")]
    public class DocumentModel
    {
        public string Id { get; set; }
        [Keyword]
        public string Name { get; set; }
        [Text]
        public string Summary { get; set; }

        [Text]
        public string Body { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var connection = new ConnectionSettings(new Uri("http://localhost:9200"));
            connection.DefaultIndex("test");
            IElasticClient client = new ElasticClient(connection);

            //if (client.IndexExists("test")?.Exists ?? false)
            //{
            //    client.DeleteIndex("test");
            //}

            var indexResponse = client.CreateIndex("test", c => c
                    .Mappings(m => m
                            .Map<DocumentModel>(p => p
                                    .AutoMap()
                            )
                    )
            );

            var doc = new DocumentModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Document Test",
                Summary = "This is the summary",
                Body = "This is the body"
            };

            var task = Task.Run(async () =>
            {
                var indexDocResponse = await client.QueryAsync(new IndexDocumentQuery<DocumentModel>(doc));
                var model = await client.QueryAsync(new FindByIdQuery<DocumentModel>(doc.Id));

                var response = await client.QueryAsync(new MatchAllSearchQuery<DocumentModel>(), "test");
            });

            task.Wait();
        }
    }

    public class MatchAllSearchQuery<T> : SearchQuery<T> where T : class
    {
        protected override SearchDescriptor<T> BuildQuery(SearchDescriptor<T> desc)
        {
            return desc.MatchAll();
        }
    }

    public abstract class SearchQuery<T> : ElasticClientQueryObject<ISearchResponse<T>> where T : class
    {
        protected override ISearchResponse<T> ExecuteCore(IElasticClient client, string index)
        {
            return client.Search<T>(desc => BuildQuery(desc).Index(index));
        }

        protected abstract SearchDescriptor<T> BuildQuery(SearchDescriptor<T> desc);

        protected override async Task<ISearchResponse<T>> ExecuteCoreAsync(IElasticClient client, string index)
        {
            return await client.SearchAsync<T>(desc => BuildQuery(desc).Index(index));
        }
    }
}
