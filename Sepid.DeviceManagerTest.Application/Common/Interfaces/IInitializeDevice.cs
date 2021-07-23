using System.Threading.Tasks;
using Hangfire;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IInitializeDeviceToServer
    {
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        void Init();

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(3600)] 
        Task EvacuationNonVitalDevice();

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(3600)] 
        Task EvacuationVitalDevice();

        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        void InitialMatchOnServer();

      
    }
}