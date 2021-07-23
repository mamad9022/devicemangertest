using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Application.Core.UserAccessGroups.Command.Upsert;
using Sepid.DeviceManagerTest.Common.RabbitMq;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IDataCollectorService
    {
        Task CollectData();
    }

    public class DataCollectorService : IDataCollectorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBusSubscribe _busSubscribe;

        public DataCollectorService(IServiceScopeFactory serviceScopeFactory, IBusSubscribe busSubscribe)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _busSubscribe = busSubscribe;
        }

        public Task CollectData()
        {
          //  _busSubscribe.Subscribe("UpdateUserAccessGroup", ProcessUserAccessGroupMessage);

            return Task.CompletedTask;
        }


        public async Task<bool> ProcessUserAccessGroupMessage(string message, IDictionary<string, object> header)
        {
            var device = JsonConvert.DeserializeObject<UpsertUserAccessGroupCommand>(message);

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            await mediator.Send(device);

            return true;
        }
    }
}