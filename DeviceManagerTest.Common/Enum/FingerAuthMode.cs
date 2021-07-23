namespace Sepid.DeviceManagerTest.Common.Enum
{
    public enum FingerAuthMode
    {
        None = 255,
        Prohibited = 254,
        BiometricOnly = 0,
        BiometricPin = 1,
    }

    public enum FaceAuthMode
    {
        None = 255,
        Prohibited = 254,
        BiometricOnly = 0,
        BiometricPin = 1,
    }

    public enum CardAuthMode
    {
        None = 255,
        Prohibited = 254,

        CardOnly = 2,
        CardBiometric = 3,
        CardPin = 4,
        CardBiometricOrPin = 5,
    }

    public enum SecurityLevel
    {
        Default = 0,
        Lower = 1,
        Low = 2,
        Normal = 3,
        High = 4,
        Higher = 5,
    }

    public enum AuthMode
    {
        Disable = 0,
        FingerOnly = 1020,
        FingerAndPassword = 1021,
        FingerOrPassword = 1022,
        PasswordOnly = 1023,
        CardOnly = 1024
    }
}