using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;

public class MovieFile
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    public List<Movie> Movies { get; set; }
    private string filePath;

    public MovieFile(string movieFilePath)
    {
        filePath = movieFilePath;
        Movies = new List<Movie>();
        ReadMoviesFromFile();
    }
    public List<Movie> FindMoviesByTitle(string title)
{
    var matches = Movies.Where(m => m.title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
    return matches;
}


    private void ReadMoviesFromFile()
    {
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var columns = line.Split(',');
                    var movie = new Movie
                    {
                        mediaId = ulong.Parse(columns[0]),
                        title = columns[1],
                        genres = columns[2].Split('|').ToList(),
                        director = columns[3],
                        runningTime = TimeSpan.Parse(columns[4])
                    };
                    Movies.Add(movie);
                }
            }
            logger.Info($"Movies in file {Movies.Count}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error reading the movie file");
        }
    }

    public bool isUniqueTitle(string title)
    {
        if (Movies.Any(m => m.title.Equals(title, StringComparison.OrdinalIgnoreCase)))
        {
            logger.Info($"Duplicate movie title: {title}");
            return false;
        }
        return true;
    }

    public void AddMovie(Movie movie)
    {
        movie.mediaId = Movies.Max(m => m.mediaId) + 1;
        Movies.Add(movie);

        try
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{movie.mediaId},{movie.title},{string.Join("|", movie.genres)},{movie.director},{movie.runningTime}");
            }
            logger.Info($"Movie added: {movie.title}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error writing to the movie file");
        }
    }
}
//Learn How to use Language Integrated Query(Linq): https://learn.microsoft.com/en-us/dotnet/csharp/linq/