using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordChatImporter
{
    public class DiscordWebhook : IDisposable
    {
        private readonly WebClient webClient;
        private static NameValueCollection values = new NameValueCollection();
        public string WebHook { get; set; }
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }

        public DiscordWebhook(string webHookUrl)
        {
            webClient = new WebClient();
            WebHook = webHookUrl;
            values.Add("username", "JOD.GG Message Importer");
            values.Add("avatar_url", "https://i.jod.gg/YUXA1/feBAKOmO20/raw.png");
        }


        public void SendMessage(string msgSend)
        {
            values.Set("content", msgSend);
            webClient.UploadValues(WebHook, values);
        }

        public void Dispose()
        {
            webClient.Dispose();
        }
    }

}
