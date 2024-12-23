namespace FAClient;

public interface IFAClient
{
    public string BaseUrl { get; }

    DetectResponce Detect(byte[] data, string name);
}
