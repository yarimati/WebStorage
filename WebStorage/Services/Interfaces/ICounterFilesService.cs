namespace WebStorage.Services.Interfaces
{
    public interface ICounterFilesService
    {
        int GetCurrentCounterFiles();
        int GetTotalCounterFiles();
        void IncreaseTotalCounter();
    }
}