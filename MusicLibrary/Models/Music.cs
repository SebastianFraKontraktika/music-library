using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicLibrary.Models;

public class Music
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long MusicId { get; set; }
    [Required]
    public required string Path { get; set; }
    [Required]
    public required string AlbumName { get; set; }
    [Required]
    public required string ArtistName { get; set; }
}