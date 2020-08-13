namespace WebStorage.Services
{
    public interface ICounterFilesService
    {
        int GetCurrentCounterFiles();
        int GetTotalCounterFiles();
        void IncreaseTotalCounter();
    }
}