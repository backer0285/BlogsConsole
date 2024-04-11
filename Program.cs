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
        logger.Info("Option \"1\" selected");
        DisplayBlogs();
    }
    else if (choice == "2")
    {
        logger.Info("Option \"2\" selected");
        AddBlog();
    }
    else if (choice == "3")
    {
        logger.Info("Option \"3\" selected");
    }
    else if (choice == "4")
    {
        logger.Info("Option \"4\" selected");
    }
} while (choice != "q" && choice != "Q");

logger.Info("Program ended");

void DisplayBlogs()
{
    // Display all Blogs from the database
    var db = new BloggingContext();
    var query = db.Blogs.OrderBy(b => b.Name);

    Console.WriteLine();
    Console.WriteLine(query.Count() + " Blogs returned");
    foreach (var item in query)
    {
        Console.WriteLine(item.Name);
    }
    Console.WriteLine();
}

void AddBlog()
{
    try
    {
        // Create and save a new Blog
        Console.WriteLine();
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        var blog = new Blog { Name = name };

        var db = new BloggingContext();
        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }
}