﻿using NLog;
using System.Data;
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
        Console.WriteLine();
        DisplayBlogs();
        Console.WriteLine();
    }
    else if (choice == "2")
    {
        logger.Info("Option \"2\" selected");
        Console.WriteLine();
        AddBlog();
        Console.WriteLine();
    }
    else if (choice == "3")
    {
        logger.Info("Option \"3\" selected");
        Console.WriteLine();
        CreatePost();
        Console.WriteLine();
    }
    else if (choice == "4")
    {
        logger.Info("Option \"4\" selected");
        Console.WriteLine();
        DisplayPosts();
        Console.WriteLine();
    }
} while (choice != "q" && choice != "Q");

logger.Info("Program ended");

void DisplayBlogs()
{
    // Display all Blogs from the database
    var db = new BloggingContext();
    var query = db.Blogs.OrderBy(b => b.Name);

    Console.WriteLine(query.Count() + " Blogs returned");
    foreach (var item in query)
    {
        Console.WriteLine(item.Name);
    }
}

void AddBlog()
{
    try
    {
        // Create and save a new Blog
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        if (!String.IsNullOrEmpty(name))
        {
            var blog = new Blog { Name = name };

            var db = new BloggingContext();
            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
        }
        else
        {
            logger.Error("Blog name cannot be null");
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }
}

void CreatePost()
{
    try
    {
        Console.WriteLine("Select the blog you would like to post to: ");

        var db = new BloggingContext();
        var blogChoice = GetBlogMenuChoice();

        if (int.TryParse(blogChoice, out int blogId))
        {
            if (db.Blogs.Any(b => b.BlogId.Equals(blogId)))
            {
                Console.WriteLine("Enter the Post title");
                string title = Console.ReadLine();

                if (!String.IsNullOrEmpty(title))
                {
                    Console.WriteLine("Enter the Post content");
                    string content = Console.ReadLine();
                    var post = new Post { Title = title, Content = content, BlogId = blogId };
                    db.AddPost(post);
                    logger.Info("Blog added - {content}", content);
                }
                else
                {
                    logger.Error("Post title cannot be null");
                }
            }
            else
            {
                logger.Error("There are no Blogs saved with that Id");
            }
        }
        else
        {
            logger.Error("Invalid Blog Id");
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }

}

void DisplayPosts()
{
    Console.WriteLine("Select the blog's posts to display: ");
    Console.WriteLine("0) Posts from all blogs");
    var db = new BloggingContext();
    var blogChoice = GetBlogMenuChoice();

    if (int.TryParse(blogChoice, out int blogId))
    {
        if (db.Blogs.Any(b => b.BlogId.Equals(blogId)) || blogId == 0)
        {
            if (blogId == 0)
            {
                var tableJoin = db.Blogs.Join(db.Posts, blog => blog.BlogId, post => post.BlogId, (blog, post) => new { BlogName = blog.Name, Title = post.Title, Content = post.Content });
                Console.WriteLine(tableJoin.Count() + " post(s) returned");
                Console.WriteLine();

                foreach (var item in tableJoin)
                {
                    Console.WriteLine("Blog: " + item.BlogName);
                    Console.WriteLine("Title: " + item.Title);
                    Console.WriteLine("Content: " + item.Content);
                    Console.WriteLine();
                }
            }
            else
            {

            }
        }
        else
        {
            logger.Error("There are no Blogs saved with that Id");
        }
    }
    else
    {
        logger.Error("Invalid Blog Id");
    }
}

string GetBlogMenuChoice()
{
    var db = new BloggingContext();
    var query = db.Blogs.OrderBy(b => b.BlogId);
    int counter = 1;
    foreach (var item in query)
    {
        Console.WriteLine(item.BlogId + ") " + item.Name);
        counter++;
    }
    return Console.ReadLine();
}
