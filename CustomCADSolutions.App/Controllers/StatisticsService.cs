using CustomCADSolutions.App.Hubs;
using CustomCADSolutions.Core.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace CustomCADSolutions.App.Controllers
{
    public class StatisticsService
    {
        private readonly ICadService cadService;
        private readonly IHubContext<CadsHub> hubContext;

        public StatisticsService(ICadService cadService, IHubContext<CadsHub> hubContext)
        {
            this.cadService = cadService;
            this.hubContext = hubContext;
        }

        public async Task SendStatistics(string userId)
        {
            int unvCads = cadService.Count(c => c.IsValidated == false);
            int userCads = cadService.Count(c => c.CreatorId == userId);

            await hubContext.Clients.All.SendAsync("ReceiveStatistics", userCads, unvCads);
        }
    }
}
