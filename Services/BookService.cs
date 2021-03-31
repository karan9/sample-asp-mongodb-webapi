#region snippet_BookServiceClass
using BooksApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        private readonly IMongoCollection<BsonDocument> _booksBSON;
        private readonly IMongoCollection<BsonDocument> _gradesBSON;

        #region snippet_BookServiceConstructor
        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            System.Console.WriteLine(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
            _booksBSON = database.GetCollection<BsonDocument>(settings.BooksCollectionName);
            _gradesBSON = database.GetCollection<BsonDocument>(settings.GradesCollectionName);
        }
        #endregion

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => 
            _books.DeleteOne(book => book.Id == id);

        public void UpdateNew(string id, string key, string value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", "6062cc253206ff140c820b3c");
            var update = Builders<BsonDocument>.Update.Set("metadata." + key, value);
            _booksBSON.UpdateOne(filter, update);
        }

        public void InsertArray()
        {
            var document = new BsonDocument
            {
                { "student_id", 10002 },
                { "scores", new BsonArray
                    {
                    new BsonDocument{ {"type", "exam"}, {"score", 88.12334193287023 } },
                    new BsonDocument{ {"type", "quiz"}, {"score", 74.92381029342834 } },
                    new BsonDocument{ {"type", "homework"}, {"score", 89.97929384290324 } },
                    new BsonDocument{ {"type", "homework"}, {"score", 82.12931030513218 } }
                    }
                },
                { "class_id", 480}
            };

            _gradesBSON.InsertOne(document);
        }

        public void UpdateArray()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 10002)
                            & Builders<BsonDocument>.Filter.Eq("scores.type", "quiz");

           // When you want to update single document
            var update = Builders<BsonDocument>.Update.Set("scores.$.score", 79.99);

            // When you want to replace the entire object
            // var update = Builders<BsonDocument>.Update.Set("scores.$", new BsonDocument("k", 123));


            var result = _gradesBSON.UpdateOne(filter, update);
        }

        public void UpdateElementInsideArray()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 10001);
            var update = Builders<BsonDocument>.Update.Set("scores.$[i].score", 80.99);
            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("i.type", "exam"))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
            var result = _gradesBSON.UpdateOne(filter, update, updateOptions);
        }
    }
}
#endregion
