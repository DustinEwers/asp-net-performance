using System.Collections.Generic;
using System.Linq;
using perfDemo.Models;

namespace perfDemo.Migrations
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedCustomers(context);
            SeedProducts(context);
            SeedOrders(context);
        }

        private void SeedProducts(ApplicationDbContext context)
        {
            var products = new[]
            {
                new Product {Name = "Plush Cheetah", Description = "This is a fine microfiber plush cheetah.", Price = 10.0m},
                new Product {Name = "Plush Sloth", Description = "This is an adorable plush sloth.", Price = 5.0m},
                new Product {Name = "Stuffed Bear", Description = "This is a non-descript stuffed bear.", Price = 15.0m},
                new Product {Name = "Gold plated Sloth Bust", Description = "This fine bust would be lovely over your fireplace.", Price = 50.0m},
            };

            if (!context.Products.Any())
            {
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

        private void SeedOrders(ApplicationDbContext context)
        {
            var customer1 = context.Customers.First(x => x.FirstName == "Rose");
            var customer2 = context.Customers.First(x => x.FirstName == "Amy");

            var plushSloth = context.Products.First(x => x.Name == "Plush Sloth");
            var plushCheetah = context.Products.First(x => x.Name == "Plush Cheetah");


            var orders = new[]
            {
                new Order
                {
                    CustomerId = customer1.Id,
                    Shipping = 2.0m,
                    Tax = 1.0m,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine { ProductId = plushCheetah.Id, Price = plushCheetah.Price }
                    }  
                },
                new Order
                {
                    CustomerId = customer2.Id,
                    Shipping = 2.0m,
                    Tax = 1.0m,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine { ProductId = plushSloth.Id, Price = plushSloth.Price }
                    }
                }


            };

            if (!context.Orders.Any())
            {
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }

        private static void SeedCustomers(ApplicationDbContext context)
        {
            var customers = new[]
            {
                new Customer
                {
                    FirstName = "Amy",
                    LastName = "Pond",
                    Address1 = "123 Street",
                    City = "Anywhere",
                    State = "WI"
                },
                new Customer
                {
                    FirstName = "Rose",
                    LastName = "Tyler",
                    Address1 = "5432 Street",
                    City = "Somewhere",
                    State = "WI"
                }
            };

            context.Customers.AddOrUpdate(c => new {c.FirstName, c.LastName}, customers);
            context.SaveChanges();
        }
    }
}
