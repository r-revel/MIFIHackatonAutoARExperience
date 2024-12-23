namespace FAClient;

internal class FAClientV1 : BaseClient, IFAClient
{
    public FAClientV1(string baseUrl) : base(baseUrl) { }

    public DetectResponce Detect(byte[] data) =>
        Post<DetectResponce>("detect", new DetectRequest() { Data = Convert.ToBase64String(data), });
}
