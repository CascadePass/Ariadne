namespace CascadePass.CascadeCore.UI
{
    public interface IRegistryProvider
    {
        object GetValue(string keyName, string valueName);
    }
}
