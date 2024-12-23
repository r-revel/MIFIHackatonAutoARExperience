namespace FAClient;

public static class FA
{
    public static IFAClient GetClient(string baseUrl)
    {
        return new FAClientV1(baseUrl);
    }
}
