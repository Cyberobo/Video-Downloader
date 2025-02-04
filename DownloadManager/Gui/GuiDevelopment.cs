using DownloadManager.AppSettings;
using DownloadManager.SocialMedias.Instagram;
using DownloadManager.SocialMedias.Youtube;
using Spectre.Console;

namespace DownloadManager.Gui
{
    internal class GuiDevelopment
    {
        public static async Task Menu()
        {
            bool confirmation = true;
            string videoUrl;
            List<string> links = new List<string>();
            
            //Set youtube directions
            Youtube.youtubeVideoDirectory = await SettingsControl.ReadPath(YoutubeMp4Path: true);
            Youtube.Mp3Directory = await SettingsControl.ReadPath(YoutubeMp3Path: true);

            //Set instagram direction
            Instagram.InstagramReelsDirectory = await SettingsControl.ReadPath(InstagramPath: true);

            AnsiConsole.Write(
             new FigletText("Download Manager")
             .Centered()
             .Color(Color.Red));

            var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("What's your [blue]selection[/]?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up or down)[/]")
            .AddChoices(new[] {
                        "Youtube", "Instagram", "File Direction Settings",//"Facebook",
                        //"Twitter", "Tiktok", "Exit"
            }));

            
            AnsiConsole.Markup($"[underline red]Selected: {selection}![/]\n");

            if (selection == "Exit")
                Environment.Exit(0);
                    

            Console.WriteLine("\n");

            switch (selection)
            {
                case "Youtube":
                    
                    var downloadType = DownloadChoice();

                    switch (downloadType)
                    {
                        case "Select Mp4 quality":

                            while (confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));

                                    links.Add(videoUrl);
                                }

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {
                                        await youtube.DownloadAndSelectResulation(link);

                                    }

                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {   
                                        //set to default mp4 path location
                                        await SettingsControl.WritePath(YoutubeMp4Path: @".\Videos\Youtube\Videos");
                                        //set to youtube mp4 directory global variable
                                        Youtube.youtubeVideoDirectory = await SettingsControl.ReadPath(YoutubeMp4Path: true);
                                        
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        case "Max Mp4 quality":
                            
                            while(confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));

                                    links.Add(videoUrl);
                                }

                                Console.WriteLine();

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {
                                        await youtube.DownloadMaxQuality(link);

                                    }
                                        
                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {
                                        //set to default mp4 path location
                                        await SettingsControl.WritePath(YoutubeMp4Path: @".\Videos\Youtube\Videos");
                                        //set to youtube mp4 directory global variable
                                        Youtube.youtubeVideoDirectory = await SettingsControl.ReadPath(YoutubeMp4Path: true);

                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        case "Min Mp4 quality":
                            
                            while (confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));

                                    links.Add(videoUrl);
                                }

                                Console.WriteLine();

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {
                                        await youtube.DownloadMinQuality(link);

                                    }

                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {
                                        //set to default mp4 path location
                                        await SettingsControl.WritePath(YoutubeMp4Path: @".\Videos\Youtube\Videos");
                                        //set to youtube mp4 directory global variable
                                        Youtube.youtubeVideoDirectory = await SettingsControl.ReadPath(YoutubeMp4Path: true);

                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        case "Mp3":

                            while (confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));

                                    links.Add(videoUrl);
                                }

                                Console.WriteLine();

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {
                                        await youtube.DownloadMp3(link);

                                    }

                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {
                                        //set to default mp3 path location
                                        await SettingsControl.WritePath(YoutubeMp3Path: @".\Videos\Youtube\Mp3");
                                        //set to youtube mp3 directory global variable
                                        Youtube.Mp3Directory = await SettingsControl.ReadPath(YoutubeMp3Path: true);

                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        case "Download Playlist (Mp4)":
                            int startRange = 0, endRange = 0;
                            bool DownloadTypeMp4 = false;
                            Dictionary<int,int> rangePlaylistMp4 = new Dictionary<int,int>();
                            int rangeQueueMp4 = 0;

                            while (confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));
                                    links.Add(videoUrl);

                                    DownloadTypeMp4 = AnsiConsole.Prompt(
                                      new TextPrompt<bool>("If you want to download videos All press [blue]a[/] or a cetian range press [blue]r[/].")
                                      .AddChoice(true)
                                      .AddChoice(false)
                                      .WithConverter(choice => choice ? "a" : "r"));
                                    Console.Write("\n");

                                    if (!DownloadTypeMp4)
                                    {
                                        startRange = AnsiConsole.Prompt(
                                        new TextPrompt<int>("Please enter the start of the range: "));
                                        endRange = AnsiConsole.Prompt(
                                        new TextPrompt<int>("Please enter the end of the range: "));
                                        rangePlaylistMp4.Add(startRange, endRange);

                                    }

                                    Console.WriteLine();
                                }

                                Console.WriteLine();

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {
                                        if (DownloadTypeMp4)
                                        {
                                            await youtube.DownloadMp4PlayList(link);
                                        }
                                        else
                                        {
                                            await youtube.DownloadMp4PlayList(link, rangePlaylistMp4.Keys.ToList()[rangeQueueMp4], rangePlaylistMp4.Values.ToList()[rangeQueueMp4]);
                                            rangeQueueMp4++;
                                        }
                                    }

                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {
                                        //set to default mp4 path location
                                        await SettingsControl.WritePath(YoutubeMp4Path: @".\Videos\Youtube\Videos");
                                        //set to youtube mp4 directory global variable
                                        Youtube.youtubeVideoDirectory = await SettingsControl.ReadPath(YoutubeMp4Path: true);

                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        case "Download Playlist (Mp3)":
                            bool DownloadTypeMp3 = false;
                            Dictionary<int, int> rangePlaylistMp3 = new Dictionary<int, int>();
                            int rangeQueueMp3 = 0;
        
                            while (confirmation)
                            {
                                int countOfLink = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many want to enter link: "));

                                for (int i = 0; i < countOfLink; i++)
                                {
                                    videoUrl = AnsiConsole.Prompt(
                                    new TextPrompt<string>("Please enter your link: "));
                                    links.Add(videoUrl);

                                    DownloadTypeMp3 = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("If you want to download videos All press [blue]a[/] or a cetian range press [blue]r[/].")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "a" : "r"));
                                    Console.Write("\n");

                                    if (!DownloadTypeMp3)
                                    {
                                        startRange = AnsiConsole.Prompt(
                                        new TextPrompt<int>("Please enter the start of the range: "));
                                        endRange = AnsiConsole.Prompt(
                                        new TextPrompt<int>("Please enter the end of the range: "));
                                        rangePlaylistMp3.Add(startRange, endRange);

                                    }

                                    Console.WriteLine();
                                }

                                Console.WriteLine();

                                try
                                {
                                    Youtube youtube = new Youtube();

                                    foreach (var link in links)
                                    {

                                        if (DownloadTypeMp3)
                                        {
                                            await youtube.DownloadMp3PlayList(link);
                                        }
                                        else
                                        {
                                            await youtube.DownloadMp3PlayList(link, rangePlaylistMp3.Keys.ToList()[rangeQueueMp3], rangePlaylistMp3.Values.ToList()[rangeQueueMp3]);
                                            rangeQueueMp3++;
                                        }
                                    }

                                    Console.Write("\n");
                                    confirmation = AnsiConsole.Prompt(
                                    new TextPrompt<bool>("Would you download video again?")
                                    .AddChoice(true)
                                    .AddChoice(false)
                                    .WithConverter(choice => choice ? "y" : "n"));
                                    Console.Write("\n");

                                    links.Clear();

                                    if (!confirmation)
                                    {
                                        Console.Clear();
                                        await Menu();
                                    }

                                }
                                catch (Exception e)
                                {
                                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                                    {
                                        //set to default mp3 path location
                                        await SettingsControl.WritePath(YoutubeMp3Path: @".\Videos\Youtube\Mp3");
                                        //set to youtube mp3 directory global variable
                                        Youtube.Mp3Directory = await SettingsControl.ReadPath(YoutubeMp3Path: true);

                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                        links.Clear();

                                    }
                                    else
                                    {
                                        AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                        links.Clear();

                                    }

                                }


                            }

                            break;
                        default:
                            AnsiConsole.Markup("[red]Error[/]");
                            break;
                    }
                    break;

                case "Instagram":

                    while (confirmation)
                    {
                        int countOfLink = AnsiConsole.Prompt(
                        new TextPrompt<int>("How many want to enter link: "));

                        for (int i = 0; i < countOfLink; i++)
                        {
                            videoUrl = AnsiConsole.Prompt(
                            new TextPrompt<string>("Please enter your link: "));

                            links.Add(videoUrl);
                        }

                        try
                        {
                            Instagram instagram = new Instagram();

                            foreach (var link in links)
                            {
                                await instagram.DownloadVideoAsync(link);
                                Task.Delay(2000);
                            }

                            Console.Write("\n");
                            confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>("Would you download video again?")
                            .AddChoice(true)
                            .AddChoice(false)
                            .WithConverter(choice => choice ? "y" : "n"));
                            Console.Write("\n");

                            links.Clear();

                            if (!confirmation)
                            {
                                Console.Clear();
                                await Menu();
                            }

                        }
                        catch (Exception e)
                        {
                            if (e is FileNotFoundException || e is DirectoryNotFoundException)
                            {
                                //set to default instagram path location
                                await SettingsControl.WritePath(InstagramPath: @".\Videos\Instagram\Reels");
                                //set to instagram reels directory global variable
                                Instagram.InstagramReelsDirectory = await SettingsControl.ReadPath(InstagramPath: true);

                                AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                AnsiConsole.Markup($"[green]path is set to default, try again!\n\n[/]");

                                links.Clear();

                            }
                            else
                            {
                                AnsiConsole.Markup($"[red]An error occurred while downloading the videos: {Markup.Escape(e.Message)}\n\n[/]");
                                links.Clear();

                            }

                        }


                    }

                    break;

                case "Facebook":
                    break;

                case "Twitter":
                    break;

                case "Tiktok":
                    break;

                case "File Direction Settings":
                    var settingsSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("What's your [blue]selection[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up or down)[/]")
                    .AddChoices(new[] {
                                "Youtube Settings", "Instagram Settings",//"Facebook Settings",
                                //"Twitter Settings", "Tiktok Settings", "Exit"
                    
                    }));

                    switch(settingsSelection)
                    {
                        case "Youtube Settings":

                            var YoutubePathDirection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("What's your [blue]selection[/]?")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up or down)[/]")
                            .AddChoices(new[] {
                                "Youtube Mp4 File Direction", "Youtube Mp3 File Direction",
                            }));

                            if(YoutubePathDirection == "Youtube Mp4 File Direction")
                            {
                                string Mp4Path = AnsiConsole.Prompt(
                                new TextPrompt<string>("Please enter your path: "));
                                await SettingsControl.WritePath(YoutubeMp4Path : Mp4Path);

                                AnsiConsole.Markup("[green]Changes Saved![/]");
                                Thread.Sleep(1000);
                                Console.Clear();
                                await Menu();
                            }
                            else
                            {
                                string Mp3Path = AnsiConsole.Prompt(
                                new TextPrompt<string>("Please enter your path: "));
                                await SettingsControl.WritePath(YoutubeMp3Path : Mp3Path);

                                AnsiConsole.Markup("[green]Changes Saved![/]");
                                Thread.Sleep(1000);
                                Console.Clear();
                                await Menu();
                            }

                            break;
                        case "Instagram Settings":

                            var InstagramPathDirection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("What's your [blue]selection[/]?")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up or down)[/]")
                            .AddChoices(new[] {
                                    "Reels File Direction",
                            }));

                            string InstagramReelsPath = AnsiConsole.Prompt(
                            new TextPrompt<string>("Please enter your path: "));
                            await SettingsControl.WritePath(InstagramPath: InstagramReelsPath);

                            AnsiConsole.Markup("[green]Changes Saved![/]");
                            Thread.Sleep(1000);
                            Console.Clear();
                            await Menu();


                            break;
                        case "Facebook Settings":
                            break;
                        case "Twitter Settings":
                            break;
                        case "Tiktok Settings":
                            break;
                        default:
                            AnsiConsole.Markup("[red]Settings Error[/]");
                            break;

                    }

                    break;

                default:
                    AnsiConsole.Markup("[red]Error[/]");
                    break;

            }

        }

        public static string DownloadChoice()
        {
             var downloadType = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("Select download [blue]type[/]?")
             .PageSize(10)
             .MoreChoicesText("[grey](Move up or down)[/]")
             .AddChoices(new[] {
                        "Select Mp4 quality", "Max Mp4 quality", "Min Mp4 quality",
                        "Mp3", "Download Playlist (Mp4)", "Download Playlist (Mp3)"
             }));

            return downloadType;

        }

        public static async Task DownloadTextAnimation(ProgressTask downloadTask)
        {
            
            bool animationControl = true;


            while(animationControl)
            {
                string dot = "";
                for (int i = 0; i < 3; i++)
                {
                    dot += ".";
                    downloadTask.Description = $"[green]Downloading{dot}[/]";
                    await Task.Delay(250);
                }
            }

        }
    }
}
