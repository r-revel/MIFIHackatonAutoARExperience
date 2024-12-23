namespace FAClient;

internal class FAClientV1 : BaseClient, IFAClient
{
    public FAClientV1(string baseUrl) : base(baseUrl) { }

    public DetectResponce Detect(byte[] data, string name) =>
        Post<DetectResponce>("detect", new DetectRequest() { Data = data, Name = name });
}
