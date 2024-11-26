using Microsoft.AspNetCore.SignalR;

namespace CognitechnaZadanie.Hubs
{
    public class TasksHub : Hub
    {
        private readonly IHubContext<TasksHub> _hubContext;

        public TasksHub(IHubContext<TasksHub> hubContext)
        {
            _hubContext = hubContext;
        }

    }
}
