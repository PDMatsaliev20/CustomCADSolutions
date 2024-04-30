using CustomCADSolutions.App.Controllers;

using Microsoft.AspNetCore.SignalR;

namespace CustomCADSolutions.App.Hubs
{
    public class CadsHub : Hub
    {
        private readonly StatisticsService statisticsService;

        public CadsHub(StatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task SendStatistics(string userId)
        {
            await statisticsService.SendStatistics(userId);
        }
    }
}
