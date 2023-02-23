using System.Text;

namespace DiscordChatImporter
{
    internal class Program
    {
        private static Settings settings;
        static void Main(string[] args)
        {
            LoadSettings();
            List<string> files = GetFiles();
            List<string> messages = GetMessages(files);

            Console.WriteLine("Sending messages...\n");
            SendMessages(messages);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nDone!");
            Console.ResetColor();
            Console.WriteLine(" All Messages sent.");
        }

        private static void LoadSettings() {
            Console.Write("Enter your Discord webhook URL: ");
            string whurl = Console.ReadLine();
            Console.Write("Enter any words you want to filter out (separated by a comma): ");
            string[] filter = Console.ReadLine().Split(',');
            settings = new Settings(whurl, filter);
        }

        private static List<string> GetFiles() {
            List<string> files = new List<string>();
            Console.WriteLine("Please enter the to the directory to import all files from: ");
            string dir = Console.ReadLine();
            if (!Directory.Exists(dir)) throw new DirectoryNotFoundException(nameof(dir));
            files.AddRange(Directory.GetFiles(dir, "*.txt"));
            return files;
        }


        private static List<string> GetMessages(List<string> files)
        {
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
            return messages;
        }

        private static void SendMessages(List<string> messages)
        {
            int counter = 0;
            foreach (string message in messages)
            {
                Thread.Sleep(400);
                Console.Write("Sending message " + ++counter + " of " + messages.Count + "...");
                settings.Webhook.SendMessage(message);
                settings.Webhook.Dispose();
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
                if (settings.Filter.Any(line.Contains)) continue;
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