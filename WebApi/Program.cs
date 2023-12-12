using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Persistence.Context;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListenForIntegrationEvents();
            CreateHostBuilder(args).Build().Run();
        }
        
        private static void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlServer(
                            "Server=DESKTOP-L7KEKFD;Database=library42;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;") // Replace "your_connection_string_here" with your actual SQL Server connection string
                        .Options;

                    var dbContext = new ApplicationDbContext(contextOptions);

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    var data = JObject.Parse(message);
                    var type = ea.RoutingKey;
                    if (type == "book.add")
                    {
                        dbContext.Books.Add(new Book()
                        {
                            Id = Guid.Parse(data["id"].Value<string>()),
                            Title = data["title"].Value<string>(),
                            Subtitle = data["subtitle"].Value<string>(),
                            Description = data["description"].Value<string>(),
                            Genre = (Genre)data["genre"].Value<int>(), // Assuming Genre is an enum
                            Publisher = data["publisher"].Value<string>(),
                            ISBN = data["isbn"].Value<string>(),
                            Rating = data["rating"].Value<double?>(),
                            ReleaseDate = data["releaseDate"].Value<DateTime>(),
                            AuthorId = Guid.Parse(data["authorId"].Value<string>())
                        });
                        dbContext.SaveChanges();
                    }
                    else if (type == "book.update")
                    {
                        var book = dbContext.Books.First(a => a.Id == data["id"].Value<Guid>());
                        book.Title = data["newTitle"].Value<string>(); // Assuming you want to update the Title property
                        book.Subtitle = data["newSubtitle"].Value<string>();
                        book.Description = data["newDescription"].Value<string>();
                        book.Genre = data["newGenre"].Value<Genre?>(); // Assuming Genre is an enum
                        book.Publisher = data["newPublisher"].Value<string>();
                        book.ISBN = data["newISBN"].Value<string>();
                        book.Rating = data["newRating"].Value<double?>();
                        book.ReleaseDate = data["newReleaseDate"].Value<DateTime>();
                        book.AuthorId = data["newAuthorId"].Value<Guid?>();
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };
            channel.BasicConsume(queue: "book_exchange.reviewservice",
                autoAck: true,
                consumer: consumer);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}