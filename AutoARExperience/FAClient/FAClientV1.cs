using FAClient.Dto;
using System.IO;

namespace FAClient;

internal class FAClientV1 : BaseClient, IFAClient
{
    public FAClientV1(string baseUrl) : base(baseUrl)
    {
    }

    public DetectionResponce Detect(byte[] data, string name)
    {
        var sData = Convert.ToBase64String(data);
        var continer = new FileData() { b64Value = sData, name = name};
        return Post<DetectionResponce>("detect", continer);
    }
}
