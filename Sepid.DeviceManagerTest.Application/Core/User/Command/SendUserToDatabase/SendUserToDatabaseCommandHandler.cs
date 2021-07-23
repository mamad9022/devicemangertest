using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.SendUserToDatabase
{
    public class SendUserToDatabaseCommandHandler : IRequestHandler<SendUserToDatabaseCommand, Result>
    {
        private readonly IBusPublish _busPublish;

        public SendUserToDatabaseCommandHandler(IBusPublish busPublish)
        {
            _busPublish = busPublish;
        }

        public Task<Result> Handle(SendUserToDatabaseCommand request, CancellationToken cancellationToken)
        {
            _busPublish.Send("UpdatePerson", JsonSerializer.Serialize(request));

            return Task.FromResult(Result.SuccessFul());
        }

       
    }
}