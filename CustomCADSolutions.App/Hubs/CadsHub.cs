using Microsoft.AspNetCore.SignalR;

namespace CustomCADSolutions.App.Hubs
{
    public class CadsHub : Hub
    {
        private readonly CadsHubHelper statisticsService;

        public CadsHub(CadsHubHelper statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task SendStatistics(string userId)
        {
            await statisticsService.SendStatistics(userId);
        }
    }
}
