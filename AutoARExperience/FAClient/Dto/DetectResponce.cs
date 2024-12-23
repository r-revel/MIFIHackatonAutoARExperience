namespace FAClient.Dto;

public class DetectResponce
{
    public float Probability { get; set; } = 0;

    public string ClassName { get; set; } = "";

    public override string ToString() => JsonConvert.SerializeObject(this);
}
