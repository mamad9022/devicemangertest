using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Sepid.DeviceManagerTest.Application.Common.Environment;
using Sepid.DeviceManagerTest.Application.Core.Files.Dto;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using File = Sepid.DeviceManagerTest.Application.Models.File;

namespace Sepid.DeviceManagerTest.Application.Core.Files.Command
{
    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, Result<FileDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CreateFileCommandHandler(IMapper mapper, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _env = env;
        }

        public async Task<Result<FileDto>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var tempPath = Path.Combine(ApplicationStaticPath.Documents, request.Files.FileName);
            var fileName = Path.GetFileNameWithoutExtension(tempPath);
            var extension = Path.GetExtension(tempPath);
            var newName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(_env.ContentRootPath, ApplicationStaticPath.Documents, newName);

            #region Save To File

            await using var fileStream = new FileStream(filePath, FileMode.Create);

            await request.Files.CopyToAsync(fileStream, cancellationToken);

            #endregion Save To File

            var file = new File()
            {
                Type = request.Type,
                Name = fileName,
                Size = request.Files.Length,
                Url = $"{ApplicationStaticPath.Clients.Document}/{newName}",
                MediaType = request.Files.ContentType,
                Path = Path.Combine(ApplicationStaticPath.Documents, newName),
                CreateDate = DateTime.Now
            };

            return Result<FileDto>.SuccessFul(_mapper.Map<FileDto>(file));
        }
    }
}