using Newtonsoft.Json.Linq;
using Spectre.Console;
using DownloadManager.SocialMedias.Instagram.InstagramExceptions;

namespace DownloadManager.SocialMedias.Instagram
{
    internal class Instagram
    {
        public static string InstagramReelsDirectory;
        private string response;
        private JObject json;

        public async Task DownloadVideoAsync(string Url)
        {
   
            //get video url
            string videoUrl = await GetVideoUrlAsync(Url);
            //get reel infos
            var ReelsInfo = await GetReelsInfo(Url);
            var outputFilePath = Path.Combine(InstagramReelsDirectory, $"{ReelsInfo.UserName + Guid.NewGuid().ToString()}.mp4");

            Console.Write("\n");

            //show video informations
            AnsiConsole.Markup(
                   $"[yellow]Author: {ReelsInfo.UserName}[/]\n" +
                   $"[yellow]Duration: {ReelsInfo.Duration}[/]\n");


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", GetRandomUserAgent());

                //Referer
                client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com/");

                //Origin
                client.DefaultRequestHeaders.Add("Origin", "https://www.instagram.com");

                // Get video data
                byte[] videoData = await client.GetByteArrayAsync(videoUrl);
                await File.WriteAllBytesAsync(outputFilePath, videoData);
                AnsiConsole.Markup("[green]Download Complate[/]\n\n");
                Thread.Sleep(1000);
            }
        }

        public async Task<string> GetVideoUrlAsync(string Url)
        {
            var jsonUrl = await ConvertJsonUrl(Url);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", GetRandomUserAgent());

                //Referer
                client.DefaultRequestHeaders.Add("Referer", "https://www.instagram.com/");

                //Origin
                client.DefaultRequestHeaders.Add("Origin", "https://www.instagram.com");

                response = await client.GetStringAsync(jsonUrl);
                //Console.WriteLine(response);
                //Console.Write("\n\n\n\n");
                json = JObject.Parse(response);

                var videoUrl = json.SelectToken("graphql.shortcode_media.video_url");

                if (videoUrl != null)
                    return videoUrl.ToString();
                else
                {
                    throw new InstagramException("Video URL not found!");
                }
;
            }

        }

        public async Task<Reels> GetReelsInfo(string Url)
        {
            var jsonUrl = await ConvertJsonUrl(Url);
            //var title = json.SelectToken("graphql.shortcode_media.title");
            var duration = json.SelectToken("graphql.shortcode_media.video_duration");
            var userName = json.SelectToken("graphql.shortcode_media.owner.username");
            //var viewCount = json.SelectToken("graphql.shortcode_media.video_view_count");
            //var likeCount;

            string formatTime = (float)duration < 60f ? duration.ToString() + " second" : ((float)duration / 60f).ToString() + " minute";

            if (duration != null || userName != null)
            {
                var ReelsInfo = new Reels()
                {
                    UserName = userName.ToString(),
                    Duration = formatTime
                };

                return ReelsInfo;
            }

            else
            {
                throw new InstagramException("Reels Info not found!");
            }
        }

        public async Task<string> ConvertJsonUrl(string Url)
        {
            string jsonUrl = Url;

            //for website
            if (jsonUrl.Contains("?utm_source=ig_web_copy_link") && !jsonUrl.Contains("?igsh="))    
                jsonUrl = jsonUrl.Replace("?utm_source=ig_web_copy_link", "");
                
            else
            {
                if(jsonUrl.Contains("reels"))
                    jsonUrl = jsonUrl.Replace("reels", "reel");
            }

            //for mobile
            if (jsonUrl.Contains("?igsh=") || jsonUrl.Contains("?igsh=") && jsonUrl.Contains("?utm_source=ig_web_copy_link"))
            {
                int index = jsonUrl.IndexOf("?igsh=");
                jsonUrl = jsonUrl.Substring(0, index);
            }

            jsonUrl += "?__a=1&__d=dis";

            return jsonUrl;
        }


        private readonly List<string> userAgents = new List<string>() 
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.140 Safari/537.36 Edge/17.17134",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36",
            "Mozilla/5.0 (Linux; Android 9; SM-G960F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.116 Mobile Safari/537.36"
        };

        public string GetRandomUserAgent()
        {
            Random random = new Random();
            int index = random.Next(userAgents.Count);
            //Console.WriteLine($"Selected: {userAgents[index]}");
            return userAgents[index];

        }
    
    
    }
}
