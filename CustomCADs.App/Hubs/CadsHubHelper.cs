using Microsoft.AspNetCore.SignalR;
using CustomCADs.Core.Contracts;

namespace CustomCADs.App.Hubs
{
    public class CadsHubHelper(ICadService cadService, IHubContext<CadsHub> hubContext)
    {
        public async Task SendStatistics(string userId)
        {
            int unvCads = cadService.Count(c => c.IsValidated == false);
            int userCads = cadService.Count(c => c.CreatorId == userId);

            await hubContext.Clients.All.SendAsync("ReceiveStatistics", userCads, unvCads);
        }
    }
}
