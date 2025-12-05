namespace CascadePass.Core.Common.Settings
{
    public interface ISettingsSerializer
    {
        string Serialize<T>(T settings);
        T Deserialize<T>(string data);
    }
}
