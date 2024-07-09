using Microsoft.AspNetCore.SignalR;

namespace CustomCADs.App.Hubs
{
    public class CadsHub(CadsHubHelper statisticsService) : Hub
    {
        public async Task SendStatistics(string userId)
        {
            await statisticsService.SendStatistics(userId);
        }
    }
}
