namespace WebStorage.Services.Interfaces
{
    public interface IGuidService
    {
        string GenLongUniqueName();
        string GenShortUniqueName();
    }
}