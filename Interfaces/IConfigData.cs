namespace pruebaFGRP.Interfaces
{
    public interface IConfigData
    {
        public Task GetConfigVar(string Key);
        public Task CheckOrderStatus();
    }
}
