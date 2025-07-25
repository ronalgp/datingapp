using System.Text.Json.Serialization;

namespace API.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }

    [JsonIgnore]
    //Navigation properties
    public Member Member { get; set; } = null!;

    public string MemberId { get; set; } = null!;
}
