using System.Threading.Tasks;
using Hangfire;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface ITransferService
    {
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        Task Retry();
    }
}