movie-organizer
===============

Organizes large movies folder by creating subdirectories of genres.

 - Requires specific naming convention to work: **Movie Title [Year].ext**
 - Files differing from this format will be moved to a directory named 'Unsorted'
 - Looks up genres on IMDB
 - Moves movie clip to folder named 'All', and creates shortcuts in each of the genre folders

To use, either place the executable and .dll dependencies in your movies folder and run there, or pass optional path argument (ie. MovieOrganizer.exe C:/Movies)