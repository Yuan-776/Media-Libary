using System;
using System.Collections.Generic;
using NLog;

class Program
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

static void Main(string[] args)
{
    logger.Info("Program started");
    string scrubbedFile = "movies.scrubbed.csv"; 
    MovieFile movieFile = new MovieFile(scrubbedFile);

    while (true)
    {
        Console.WriteLine("1) Add Movie");
        Console.WriteLine("2) Display All Movies");
        Console.WriteLine("3) Find Movie");
        Console.WriteLine("Enter to quit");
        var choice = Console.ReadLine();
        if (string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                AddMovie(movieFile);
                break;
            case "2":
                DisplayAllMovies(movieFile);
                break;
            case "3":
                FindMovie(movieFile);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    logger.Info("Program ended");
}

static void FindMovie(MovieFile movieFile)
{
    Console.WriteLine("Enter movie title to search:");
    string searchQuery = Console.ReadLine();
    var matchingMovies = movieFile.FindMoviesByTitle(searchQuery);
    if (matchingMovies.Any())
    {
        Console.WriteLine($"Found {matchingMovies.Count} matches:");
        foreach (var movie in matchingMovies)
        {
            Console.WriteLine(movie.Display());
        }
    }
    else
    {
        Console.WriteLine("No matches found.");
    }
}

    static void AddMovie(MovieFile movieFile)
    {
        Console.WriteLine("Enter movie title:");
        string title = Console.ReadLine();

        if (!movieFile.isUniqueTitle(title))
        {
            Console.WriteLine("Movie title already exists.");
            return;
        }

        List<string> genres = new List<string>();
        string input;
        do
        {
            Console.WriteLine("Enter genre (or 'done' to finish):");
            input = Console.ReadLine();
            if (input.ToLower() != "done" && !string.IsNullOrWhiteSpace(input))
            {
                genres.Add(input);
            }
        } while (input.ToLower() != "done");

        Console.WriteLine("Enter director:");
        string director = Console.ReadLine();

        Console.WriteLine("Enter running time (h:m:s):");
        TimeSpan runningTime;
        while (!TimeSpan.TryParse(Console.ReadLine(), out runningTime))
        {
            Console.WriteLine("Invalid format. Please enter running time (h:m:s):");
        }

        Movie newMovie = new Movie
        {
            title = title,
            genres = genres,
            director = director,
            runningTime = runningTime
        };

        movieFile.AddMovie(newMovie);
        Console.WriteLine("Movie added successfully.");
    }

    static void DisplayAllMovies(MovieFile movieFile)
    {
        foreach (Movie movie in movieFile.Movies)
        {
            Console.WriteLine(movie.Display());
        }
    }
}
