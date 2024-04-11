﻿using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");



string choice = "";
do
{
    Console.WriteLine("Enter your selction: ");
    Console.WriteLine("1. Display all blogs");
    Console.WriteLine("2. Add blog");
    Console.WriteLine("3. Create post");
    Console.WriteLine("4. Display posts");
    Console.WriteLine("Enter q to quit");
    choice = Console.ReadLine();

    if (choice == "1")
    {

    }
    else if (choice == "2")
    {
        addBlog();
    }
    else if (choice == "3")
    {

    }
    else if (choice == "4")
    {

    }
} while (choice != "q" && choice != "Q");

logger.Info("Program ended");



void addBlog()
{
    try
    {
        // Create and save a new Blog
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        var blog = new Blog { Name = name };

        var db = new BloggingContext();
        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);

        // Display all Blogs from the database
        var query = db.Blogs.OrderBy(b => b.Name);

        Console.WriteLine("All blogs in the database:");
        foreach (var item in query)
        {
            Console.WriteLine(item.Name);
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }
}