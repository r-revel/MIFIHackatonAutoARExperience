using FAClient.Dto;

namespace FAClient;

public interface IFAClient
{
    public string BaseUrl { get; }

    DetectionResponce Detect(byte[] data, string name);
}
