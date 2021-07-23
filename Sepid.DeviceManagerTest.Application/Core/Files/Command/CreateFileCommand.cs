using MediatR;
using Microsoft.AspNetCore.Http;
using Sepid.DeviceManagerTest.Application.Core.Files.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Files.Command
{
    public class CreateFileCommand : IRequest<Result<FileDto>>
    {
        public IFormFile Files { get; set; }
        public string Type { get; set; }
    }
}