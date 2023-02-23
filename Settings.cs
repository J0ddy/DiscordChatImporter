namespace DiscordChatImporter 
{
    public class Settings
    {
        public string WebhookUrl { get; set; }
        public string[] Filter { get; set; }
        public DiscordWebhook Webhook { get; set; }

        public Settings(string webhookUrl)
        {
            WebhookUrl = webhookUrl;
            Webhook = new DiscordWebhook(webhookUrl);
            Filter = new string[] { };
        }

        public Settings(string webhookUrl, string[] filter)
        {
            WebhookUrl = webhookUrl;
            Webhook = new DiscordWebhook(webhookUrl);
            Filter = filter;
        }
    }

    // TODO: Save settings to a file and load them from there
}