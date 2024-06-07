using Microsoft.AspNetCore.SignalR;

namespace CustomCADSolutions.App.Hubs
{
    public class CadsHub(CadsHubHelper statisticsService) : Hub
    {
        public async Task SendStatistics(string userId)
        {
            await statisticsService.SendStatistics(userId);
        }
    }
}
