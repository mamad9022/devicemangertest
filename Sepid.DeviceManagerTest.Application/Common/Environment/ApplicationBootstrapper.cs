using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Common.Helper;

namespace Sepid.DeviceManagerTest.Application.Common.Environment
{
    public class ApplicationBootstrapper : IApplicationBootstrapper
    {
        private readonly IWebHostEnvironment _environment;

        public ApplicationBootstrapper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private void EnsureFoldersCreated()
        {
            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Avatars)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Avatars));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Images)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Images));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Videos)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Videos));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Musics)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Musics));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Documents)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Documents));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Others)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Others));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Db)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Db));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, "Resources")))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, "Resources"));

        }

        private void AddActivationKey()
        {

            var serial = Utils.GetSystemId();

            var path = Path.Combine("Resources", "activation.txt");

            StringBuilder activation = new StringBuilder();

            activation.AppendLine($"SystemId:{EncryptProvider.Sha256(serial)}");


            var key = "BDE6282501A84E0DB9E777D0AE37FED4";
            string Iv = "2A5658A3A7204E19";

            activation.AppendLine($"activationKey:{EncryptProvider.AESEncrypt(serial, key, Iv)}");

            File.WriteAllText(path, activation.ToString());
        }

        public void Initial()
        {
            EnsureFoldersCreated();
            AddActivationKey();
        }
    }
}