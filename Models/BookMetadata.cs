using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#region snippet_NewtonsoftJsonImport
using Newtonsoft.Json;
#endregion

namespace BooksApi.Models
{
    public class BookMetadata
    {
        [BsonExtraElements]
        public IDictionary<string, object> Metadata { get; set; }
    }
}
