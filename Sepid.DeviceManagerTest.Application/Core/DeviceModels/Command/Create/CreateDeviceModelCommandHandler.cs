using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.DeviceModels.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceModels.Command.Create
{
    public class CreateDeviceModelCommandHandler : IRequestHandler<CreateDeviceModelCommand, Result<DeviceModelDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IBusPublish _bus;
        private readonly IDistributedCache _cache;

        public CreateDeviceModelCommandHandler(IDeviceManagerContext context, IMapper mapper, IBusPublish bus, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _bus = bus;
            _cache = cache;
        }

        public async Task<Result<DeviceModelDto>> Handle(CreateDeviceModelCommand request, CancellationToken cancellationToken)
        {
            var deviceModel = _mapper.Map<DeviceModel>(request);

            await _context.DeviceModels.AddAsync(deviceModel, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            _bus.Send("deviceModel", JsonSerializer.Serialize(deviceModel));

            await _cache.RemoveAsync("getAllDeviceModels", cancellationToken);

            return Result<DeviceModelDto>.SuccessFul(_mapper.Map<DeviceModelDto>(deviceModel));
        }
    }
}