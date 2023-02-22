using System.Text;

namespace DiscordChatImporter
{
    internal class Program
    {
        private static DiscordWebhook webhook;
        private const string whurl = @"https://discord.com/api/webhooks/YOUR-WEBHOOK-HERE";
        private static readonly string[] filter = { "**Embed:** null", "Embed: **null**" }; // Filter out certain messages

        static void Main(string[] args)
        {
            List<string> files = new List<string>();
            Console.WriteLine("Please enter the to the directory to import all files from: ");
            string dir = Console.ReadLine();
            if (!Directory.Exists(dir)) throw new DirectoryNotFoundException(nameof(dir));
            files.AddRange(Directory.GetFiles(dir, "*.txt"));

            List<string> messages = new List<string>();

            foreach (string f in files)
            {
                string name = Path.GetFileName(f);
                Console.Write("Reading " + name + "...");
                messages.AddRange(ReadAndSplitFile(f));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Loaded!");
                Console.ResetColor();
            }

            Console.Write("Initializing Discord webhook...");
            webhook = new DiscordWebhook(whurl);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Done!");
            Console.ResetColor();
            Console.WriteLine("Sending messages...\n");
            SendMessages(messages);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nDone!");
            Console.ResetColor();
            Console.WriteLine(" All Messages sent.");

        }

        private static void SendMessages(List<string> messages)
        {
            int counter = 0;
            foreach (string message in messages)
            {
                Thread.Sleep(400);
                Console.Write("Sending message " + ++counter + " of " + messages.Count + "...");
                webhook.SendMessage(message);
                webhook.Dispose();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Sent!");
                Console.ResetColor();
            }
        }

        private static List<string> ReadAndSplitFile(string filePath)
        {
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            using StreamReader sr = new StreamReader(filePath);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (filter.Any(line.Contains)) continue;
                if (sb.Length + line.Length <= 1998)
                {
                    sb.Append(line+Environment.NewLine);
                    if (sr.EndOfStream)
                    {
                        lines.Add(line);
                        return lines;
                    }
                }
                else
                {
                    lines.Add(sb.ToString());
                    sb = new StringBuilder();
                }
            }

            return lines;
        }
    }
}