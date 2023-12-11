using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

/// <summary> This is stored in our DB, but there are members we don't want to pass. So we automap them to <see cref="PhotoDTO.cs"/> </summary>
[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
