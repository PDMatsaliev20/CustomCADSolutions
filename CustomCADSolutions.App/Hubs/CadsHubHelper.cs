using Microsoft.AspNetCore.SignalR;
using CustomCADSolutions.Core.Contracts;

namespace CustomCADSolutions.App.Hubs
{
    public class CadsHubHelper
    {
        private readonly ICadService cadService;
        private readonly IHubContext<CadsHub> hubContext;

        public CadsHubHelper(ICadService cadService, IHubContext<CadsHub> hubContext)
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
