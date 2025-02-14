using Newtonsoft.Json;
using Spectre.Console;

namespace DownloadManager.AppSettings
{
    internal class SettingsControl
    {
        public string? YoutubeMp4Path { get; set; }
        public string? YoutubeMp3Path { get; set; }
        public string? InstagramPath { get; set; }
        public string? FacebookPath { get; set; }
        public string? TwitterPath { get; set; }
        public string? TiktokPath { get; set; }

        private static string jsonPath = "Settings.json";
        private static JsonStructure data = new JsonStructure();
        public static async Task WritePath(string? YoutubeMp4Path = null, string? YoutubeMp3Path = null, string? InstagramPath = null, string? FacebookPath = null, string? TwitterPath = null, string ?TiktokPath = null)
        {
            await ReadPath();

            if (!File.Exists(jsonPath))
            {
                AnsiConsole.Markup("[red]Settings file not found![/]");
                data = new JsonStructure();
            }
                
            else
            {
                if (YoutubeMp4Path != null)
                    data.YoutubeMp4Path = YoutubeMp4Path;

                if (YoutubeMp3Path != null)
                    data.YoutubeMp3Path = YoutubeMp3Path;

                if (InstagramPath != null)
                    data.InstagramPath = InstagramPath;

                if (FacebookPath != null)
                    data.FacebookPath = FacebookPath;

                if (TwitterPath != null)
                    data.TwitterPath = TwitterPath;

                if (TiktokPath != null)
                    data.TiktokPath = TiktokPath;

                string updateJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                await File.WriteAllTextAsync(jsonPath, updateJson);
            
            }


        }

        public static async Task<string> ReadPath(bool YoutubeMp4Path = false, bool YoutubeMp3Path = false, bool InstagramPath = false, bool FacebookPath = false, bool TwitterPath = false, bool TiktokPath = false)
        {
            if (!File.Exists(jsonPath))
            {
                AnsiConsole.Markup("[red]Settings file not found![/]");
                data = new JsonStructure();

                return ""; // Dosya bulunamazsa boş döner

            }

            using (StreamReader sr = new StreamReader(jsonPath))
            {
                string json = await sr.ReadToEndAsync();
                data = JsonConvert.DeserializeObject<JsonStructure>(json)!;

                if (YoutubeMp4Path)
                    return data.YoutubeMp4Path;

                if (YoutubeMp3Path)
                    return data.YoutubeMp3Path;

                if (InstagramPath)
                    return data.InstagramPath;

                if (FacebookPath)
                    return data.FacebookPath;

                if (TwitterPath)
                    return data.TwitterPath;

                if (TiktokPath)
                    return data.TiktokPath;

                return "";

            }
        }

    }

    internal sealed class JsonStructure
    {

        public string? YoutubeMp4Path { get; set; }
        public string? YoutubeMp3Path { get; set; }
        public string? InstagramPath { get; set; }
        public string? FacebookPath { get; set; }
        public string? TwitterPath { get; set; }
        public string? TiktokPath { get; set; }

    }

}
