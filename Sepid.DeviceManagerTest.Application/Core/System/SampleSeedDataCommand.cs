using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.System
{
    public class SampleSeedDataCommand : IRequest
    {
    }

    public class SampleSeedDataCommandHandler : IRequestHandler<SampleSeedDataCommand>
    {
        private readonly IDeviceManagerContext _context;

        public SampleSeedDataCommandHandler(IDeviceManagerContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SampleSeedDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new SeedData(_context);

            await seeder.SeedAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}