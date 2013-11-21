using System;
using System.IO;
using System.Text.RegularExpressions;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace MovieOrganizer
{
    public class Organizer
    {
        private readonly string _root;

        public Organizer(string root)
        {
            _root = root;
            var moviesFolder = Directory.GetFiles(root);
            var regex = new Regex(@"^(?<name>(\w+ [ ]*)+)\[\d+].\w+", RegexOptions.Compiled);
            var api = new ImdbApi.ImdbApi();

            CreateDirectoryIfDoesntExist("All");
            CreateDirectoryIfDoesntExist("Unsorted");

            foreach (var path in moviesFolder)
            {
                try
                {
                    var filename = Path.GetFileName(path);
                    var extension = Path.GetExtension(path);
                    if (extension == ".dll" || extension == ".exe") continue;
                    var match = regex.Match(filename);
                    var movieName = match.Groups["name"].Value;

                    Console.WriteLine("File: " + filename);
                    if (!string.IsNullOrEmpty(movieName))
                    {
                        Console.WriteLine("Found movie: " + movieName);
                        var info = api.GetMovieInfo(movieName);
                        if (string.IsNullOrEmpty(info.Error))
                        {
                            var newPath = MoveFilePath("All", filename);
                            File.Move(path, newPath);

                            foreach (var genre in info.GenresArray)
                            {
                                CreateDirectoryIfDoesntExist(genre);
                                MakeShortcut(newPath, MoveFilePath(genre, filename));
                            }

                            continue;
                        }
                    }
                    File.Move(path, MoveFilePath("Unsorted", filename));
                    Console.WriteLine("Moved to /Unsorted");
                    Console.WriteLine("");
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed for file: {0}\n", path);
                }
            }
        }

        private void CreateDirectoryIfDoesntExist(string dir)
        {
            var path = Path.Combine(_root, dir);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private string MoveFilePath(string dir, string filename)
        {
            return Path.Combine(_root, dir, filename);
        }

        private void MakeShortcut(string path, string shortcutDirectory)
        {
            var wsh = new WshShellClass();
            var shortcut = wsh.CreateShortcut(Path.ChangeExtension(shortcutDirectory, ".lnk")) as IWshShortcut;
            shortcut.TargetPath = path;
            shortcut.Save();
        }
    }
}