namespace WebStorage.Services
{
    public interface IGuidService
    {
        string GenLongUniqueName();
        string GenShortUniqueName();
    }
}