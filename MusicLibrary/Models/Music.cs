using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MusicLibrary.Models;

public class Music
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long MusicId { get; set; }
    [Required]
    public required string Path { get; set; }
    [Required]
    [StringLength(250)]
    public required string AlbumName { get; set; }
    [Required]
    [StringLength(250)]
    public required string ArtistName { get; set; }
    
    public ICollection<MusicGenre> MusicGenres { get; set; } = new List<MusicGenre>();
}