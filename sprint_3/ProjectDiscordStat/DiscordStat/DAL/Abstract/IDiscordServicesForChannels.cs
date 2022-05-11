using DiscordStats.Models;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordServicesForChannels
    {
        Task<List<ServerChannelsVM>> GetServerChannels(string botToken, string serverId);
        Task<Channel?> CreateChannel(string botToken, string serverId, string channelName, string type, string parentId);
        Task<string?> DeleteChannel(string botToken, string channelId);
        Task<List<WebhookUsageVM>?> GetChannelWebHooks(string botToken, string channelId);
        Task<string?> CreateWebhook(string botToken, string channelId, string webhookName);
        Task<string?> ChannelEntryAndUpdateDbCheck(Channel[] channel);
        Task<string?> SendMessageThroughWebhook(string botToken, string webhookId, string webhookToken, string message);
        Task<string?> DeleteWebhook(string botToken, string webhookId);
    }
}