using Spectre.Console;
using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;
using DownloadManager.AppSettings;

namespace DownloadManager.SocialMedias.Youtube
{
    internal class Youtube
    {   
        public static string youtubeVideoDirectory; 
        public static string Mp3Directory; 
                  
        public async Task DownloadAndSelectResulation(string Url)
        {
            
            Console.Clear();

            AnsiConsole.Write(
            new FigletText("Download Manager")
            .Centered()
            .Color(Color.Red));

            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(Url);
            string cleanFileName = CleanFileName(video.Title);
            var outputFilePath = Path.Combine(youtubeVideoDirectory, $"{cleanFileName}.mp4");

            // Sanitize the video title to remove invalid characters from the file name
            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

            // Get all available muxed streams
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
            var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

            var selectedQuality = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Select your [blue]resolution:[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up or down)[/]")
            .AddChoices(videoQualities));

            //get videoStreamInfo
            var videoStreamInfo = streamManifest
            .GetVideoStreams()
            .Where(s => s.Container == Container.Mp4)
            .FirstOrDefault(s => s.VideoQuality.Label == selectedQuality) ?? throw new ("Video stream not found!");

            //get best audio quality
            var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.Mp4)
            ?.GetWithHighestBitrate() ?? throw new("Video's Audio stream not found!");

            //show video informations
            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                   $"[yellow]Author: {video.Author}[/]\n" +
                   $"[yellow]Duration: {video.Duration}[/]\n" +
                   $"[yellow]Quality: {videoStreamInfo.VideoQuality.Label}[/]\n");

            await AnsiConsole.Progress()
           .StartAsync(async ctx =>
           {
               var downloadTask = ctx.AddTask("[green]Downloading...[/]");

               var progressHandler = new Progress<double>(value =>
               {
                   downloadTask.MaxValue = 100;
                   downloadTask.Value = value * 100;
               });

               
               var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
               await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder(outputFilePath).Build(), progressHandler);
               downloadTask.Value = 100;

               if (downloadTask.Value >= 100)
                   downloadTask.Description = "[green]Download Complated![/]";
           });

            Thread.Sleep(1000);

            
        }

        public async Task DownloadMaxQuality(string Url)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(Url);
            string cleanFileName = CleanFileName(video.Title);
            var outputFilePath = Path.Combine(youtubeVideoDirectory, $"{cleanFileName}.mp4");

            //Console.WriteLine("\n\n\n\n"+youtubeVideoDirectory+"\n\n\n\n\n");

            // Sanitize the video title to remove invalid characters from the file name
            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

            // Get all available muxed streams
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
            var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

            //get videoStreamInfo
            var videoStreamInfo = streamManifest
            .GetVideoStreams()
            .Where(s => s.Container == Container.Mp4)
            .FirstOrDefault(s => s.VideoQuality.Label == "1080p60" || s.VideoQuality.Label == "1080p" || s.VideoQuality.Label == "720p60" || s.VideoQuality.Label == "720p" || s.VideoQuality.Label == "480p" || s.VideoQuality.Label == "360p" || s.VideoQuality.Label == "240p" || s.VideoQuality.Label == "144p")
            ?? throw new("Video stream not found!");

            //get best audio quality
            var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.Mp4)
            ?.GetWithHighestBitrate() ?? throw new("Video's Audio stream not found!");

            //show video informations
            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                   $"[yellow]Author: {video.Author}[/]\n" +
                   $"[yellow]Duration: {video.Duration}[/]\n" +
                   $"[yellow]Quality: {videoStreamInfo.VideoQuality.Label}[/]\n");

            await AnsiConsole.Progress()
             .StartAsync(async ctx =>
             {
                 var downloadTask = ctx.AddTask("[green]Downloading...[/]");

                 var progressHandler = new Progress<double>(value =>
                 {
                     downloadTask.MaxValue = 100;
                     downloadTask.Value = value * 100;
                 });


                 var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
                 await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder(outputFilePath).Build(), progressHandler);
                 downloadTask.Value = 100;

                 if (downloadTask.Value >= 100)
                     downloadTask.Description = "[green]Download Complated![/]";
             });

            Thread.Sleep(1000);

        }

        public async Task DownloadMinQuality(string Url)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(Url);
            string cleanFileName = CleanFileName(video.Title);
            var outputFilePath = Path.Combine(youtubeVideoDirectory, $"{cleanFileName}.mp4");

            // Sanitize the video title to remove invalid characters from the file name
            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

            // Get all available video streams
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
            var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

            //get videoStreamInfo
            var videoStreamInfo = streamManifest
            .GetVideoStreams()
            .Where(s => s.Container == Container.Mp4)
            .FirstOrDefault(s => s.VideoQuality.Label == videoQualities.LastOrDefault()) ?? throw new("Video stream not found!");

            //get best audio quality
            var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.Mp4)
            ?.GetWithHighestBitrate() ?? throw new("Video's Audio stream not found!");

            //show video informations
            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                    $"[yellow]Author: {video.Author}[/]\n" +
                    $"[yellow]Duration: {video.Duration}[/]\n" +
                    $"[yellow]Quality: {videoStreamInfo.VideoQuality.Label}[/]\n");

            await AnsiConsole.Progress()
             .StartAsync(async ctx =>
             {
                 var downloadTask = ctx.AddTask("[green]Downloading...[/]");

                 var progressHandler = new Progress<double>(value =>
                 {
                     downloadTask.MaxValue = 100;
                     downloadTask.Value = value * 100;
                 });


                 var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
                 await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder(outputFilePath).Build(), progressHandler);
                 downloadTask.Value = 100;

                 if (downloadTask.Value >= 100)
                     downloadTask.Description = "[green]Download Complated![/]";
             });

            Thread.Sleep(1000);

        }

        public async Task DownloadMp3(string Url)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(Url);

            string cleanFileName = CleanFileName(video.Title);
            var audioFilePath = Path.Combine(Mp3Directory, $"{cleanFileName}.mp3");

            Console.Write("\n");
            //show video informations
            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                 $"[yellow]Author: {video.Author}[/]\n" +
                 $"[yellow]Duration: {video.Duration}[/]\n");

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

            //get best audio quality
            var audioStreamInfo = streamManifest
            .GetAudioStreams()
            .Where(s => s.Container == Container.Mp4)
            ?.GetWithHighestBitrate() ?? throw new("Audio stream not found!");

            //await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath);

            await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
              var downloadTask = ctx.AddTask("[green]Downloading...[/]");

              var progressHandler = new Progress<double>(value =>
              {
                  downloadTask.MaxValue = 100; 
                  downloadTask.Value = value * 100; 
              });

              await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath, progressHandler);
              if (downloadTask.Value >= 100)
                downloadTask.Description = "[green]Download Complated![/]";

            });

            Thread.Sleep(1000);

        }

        public async Task DownloadMp3PlayList(string Url, int startRange = 0, int endRange = 0)
        {
            var youtube = new YoutubeClient();
            
            if(startRange == 0 && endRange == 0)
            {
                await foreach (var video in youtube.Playlists.GetVideosAsync(Url))
                {
                    try
                    {
                        string cleanFileName = CleanFileName(video.Title);
                        var audioFilePath = Path.Combine(Mp3Directory, $"{cleanFileName}.mp3");
                        string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
                        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                        var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
                        var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

                        //get best audio quality
                        var audioStreamInfo = streamManifest
                        .GetAudioStreams()
                        .Where(s => s.Container == Container.Mp4)
                        ?.GetWithHighestBitrate() ?? throw new("Audio stream not found!");

                        //show video informations
                        AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                             $"[yellow]Author: {video.Author}[/]\n" +
                             $"[yellow]Duration: {video.Duration}[/]\n");

                        await AnsiConsole.Progress()
                        .StartAsync(async ctx =>
                        {
                            var downloadTask = ctx.AddTask("[green]Downloading...[/]");

                            var progressHandler = new Progress<double>(value =>
                            {
                                downloadTask.MaxValue = 100;
                                downloadTask.Value = value * 100;
                            });

                            await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath, progressHandler);

                            if (downloadTask.Value >= 100)
                                downloadTask.Description = "[green]Download Complated![/]";
                        });

                        Thread.Sleep(1000);


                    }

                    catch (Exception e)
                    {
                        AnsiConsole.Markup($"[red]One video downlaod failed![/] {e.Message}\n\n");

                    }
                }
            }
            else
            {
                int i = 1;

                await foreach (var video in youtube.Playlists.GetVideosAsync(Url))
                {
                    if (i >= startRange)
                    {
                        try
                        {
                            string cleanFileName = CleanFileName(video.Title);
                            var audioFilePath = Path.Combine(Mp3Directory, $"{cleanFileName}.mp3");
                            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
                            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                            var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
                            var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

                            //get best audio quality
                            var audioStreamInfo = streamManifest
                            .GetAudioStreams()
                            .Where(s => s.Container == Container.Mp4)
                            ?.GetWithHighestBitrate() ?? throw new("Audio stream not found!");

                            if (audioStreamInfo == null)
                                AnsiConsole.Markup("[red]Video or Audio is not available![/]");

                            //show video informations
                            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                                  $"[yellow]Author: {video.Author}[/]\n" +
                                  $"[yellow]Duration: {video.Duration}[/]\n");
             
                            await AnsiConsole.Progress()
                            .StartAsync(async ctx =>
                            {
                                var downloadTask = ctx.AddTask("[green]Downloading...[/]");

                                var progressHandler = new Progress<double>(value =>
                                {
                                    downloadTask.MaxValue = 100;
                                    downloadTask.Value = value * 100;
                                });

                                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath, progressHandler);

                                if (downloadTask.Value >= 100)
                                    downloadTask.Description = "[green]Download Complated![/]";
                            });

                            Thread.Sleep(1000);


                        }

                        catch (Exception e)
                        {
                            AnsiConsole.Markup($"[red]One video downlaod failed![/] {e.Message}\n\n");

                        }

                    }

                    if (i == endRange)
                        break;

                    if ((startRange == i && endRange == 0) || (startRange == i && endRange == i))
                        break;


                    i++;
                }
            }


        }

        public async Task DownloadMp4PlayList(string Url, int startRange = 0, int endRange = 0)
        {
            var youtube = new YoutubeClient();

            if(startRange == 0 && endRange == 0)
            {
                await foreach (var video in youtube.Playlists.GetVideosAsync(Url))
                {
                    try
                    {
                        string cleanFileName = CleanFileName(video.Title);
                        var outputFilePath = Path.Combine(youtubeVideoDirectory, $"{cleanFileName}.mp4");
                        string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
                        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                        var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
                        var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

                        //get videoStreamInfo
                        var videoStreamInfo = streamManifest
                        .GetVideoStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .FirstOrDefault(s => s.VideoQuality.Label == "1080p60" || s.VideoQuality.Label == "1080p" || s.VideoQuality.Label == "720p60" || s.VideoQuality.Label == "720p" || s.VideoQuality.Label == "480p" || s.VideoQuality.Label == "360p" || s.VideoQuality.Label == "240p" || s.VideoQuality.Label == "144p")
                        ?? throw new("Video stream not found!");

                        //get best audio quality
                        var audioStreamInfo = streamManifest
                        .GetAudioStreams()
                        .Where(s => s.Container == Container.Mp4)
                        ?.GetWithHighestBitrate() ?? throw new("Video's Audio stream not found!");

                        if (videoStreamInfo == null || audioStreamInfo == null)
                            AnsiConsole.Markup("[red]Video or Audio is not available![/]");


                        //show video informations
                        AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                                $"[yellow]Author: {video.Author}[/]\n" +
                                $"[yellow]Duration: {video.Duration}[/]\n" +
                                $"[yellow]Quality: {videoStreamInfo.VideoQuality.Label}[/]\n");


                        await AnsiConsole.Progress()
                        .StartAsync(async ctx =>
                        {
                            var downloadTask = ctx.AddTask($"[green]Downloading...[/]");

                            //Thread animationThread = new Thread(async () =>
                            //{
                            //    GuiDevelopment.DownloadTextAnimation(downloadTask);
                            //});

                            //animationThread.Start();

                            var progressHandler = new Progress<double>(value =>
                            {
                                downloadTask.MaxValue = 100;
                                downloadTask.Value = value * 100;
                            });


                            var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
                            await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder(outputFilePath).Build(), progressHandler);
                            downloadTask.Value = 100;

                            if (downloadTask.Value >= 100)
                                downloadTask.Description = "[green]Download Complated![/]";

                            //animationThread.Join();

                        });

                        Thread.Sleep(1000);

                    }
                    catch (Exception e)
                    {
                        AnsiConsole.Markup($"[red]One video downlaod failed! {e.Message}[/]\n\n");

                    }
                }
            }
            else
            {
                int i = 1;
                
                await foreach(var video in youtube.Playlists.GetVideosAsync(Url))
                {
                    if(i >= startRange)
                    {
                        try
                        {
                            string cleanFileName = CleanFileName(video.Title);
                            var outputFilePath = Path.Combine(youtubeVideoDirectory, $"{cleanFileName}.mp4");
                            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));
                            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                            var videoStreams = streamManifest.GetVideoStreams().OrderByDescending(s => s.VideoQuality).ToList();
                            var videoQualities = videoStreams.Select(i => i.VideoQuality.Label).Distinct().ToList();

                            //get videoStreamInfo
                            var videoStreamInfo = streamManifest
                            .GetVideoStreams()
                            .Where(s => s.Container == Container.Mp4)
                            .FirstOrDefault(s => s.VideoQuality.Label == "1080p60" || s.VideoQuality.Label == "1080p" || s.VideoQuality.Label == "720p60" || s.VideoQuality.Label == "720p" || s.VideoQuality.Label == "480p" || s.VideoQuality.Label == "360p" || s.VideoQuality.Label == "240p" || s.VideoQuality.Label == "144p")
                            ?? throw new("Video stream not found!");

                            //get best audio quality
                            var audioStreamInfo = streamManifest
                            .GetAudioStreams()
                            .Where(s => s.Container == Container.Mp4)
                            ?.GetWithHighestBitrate() ?? throw new("Video's Audio stream not found!");

                            if (videoStreamInfo == null || audioStreamInfo == null)
                                AnsiConsole.Markup("[red]Video or Audio is not available![/]");


                            //show video informations
                            AnsiConsole.Markup($"[yellow]Title: {Markup.Escape(video.Title)}[/]\n" +
                                   $"[yellow]Author: {video.Author}[/]\n" +
                                   $"[yellow]Duration: {video.Duration}[/]\n" +
                                   $"[yellow]Quality: {videoStreamInfo.VideoQuality.Label}[/]\n");
                            
                            await AnsiConsole.Progress()
                            .StartAsync(async ctx =>
                            {
                                var downloadTask = ctx.AddTask("[green]Downloading...[/]");

                                var progressHandler = new Progress<double>(value =>
                                {
                                    downloadTask.MaxValue = 100;
                                    downloadTask.Value = value * 100;
                                });


                                var streamInfos = new IStreamInfo[] { audioStreamInfo, videoStreamInfo };
                                await youtube.Videos.DownloadAsync(streamInfos, new ConversionRequestBuilder(outputFilePath).Build(), progressHandler);
                                downloadTask.Value = 100;

                                if (downloadTask.Value >= 100)
                                    downloadTask.Description = "[green]Download Complated![/]";
                            });

                            Thread.Sleep(1000);

                        }
                        catch (Exception e)
                        {
                            AnsiConsole.Markup($"[red]One video downlaod failed! {e.Message}[/]\n\n");

                        }

                    }

                    if (i == endRange)
                        break;

                    if ((startRange == i && endRange == 0) || (startRange == i && endRange == i))
                        break;

                    i++;
                }

            }
        }

        public static string CleanFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string regexSearch = $"[{Regex.Escape(invalidChars)}]";
            string cleanFileName = Regex.Replace(fileName, regexSearch, "");

            return cleanFileName;
        }

    }
}
