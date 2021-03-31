using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#region snippet_NewtonsoftJsonImport
using Newtonsoft.Json;
#endregion

namespace BooksApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        #region snippet_BookNameProperty
        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string BookName { get; set; }
        #endregion

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public BookMetadata metadata { get; set; }
        //[BsonExtraElements]
        //public IDictionary<string, object> Metadata { get; set; }
    }
}
