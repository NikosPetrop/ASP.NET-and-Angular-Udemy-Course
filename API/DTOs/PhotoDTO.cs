namespace API.DTOs;

/// <summary> This is the layer that we automap and then send to responses from <see cref="Photo.cs"/> </summary>
public class PhotoDTO
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }
    public int AppUserId { get; set; }
}
