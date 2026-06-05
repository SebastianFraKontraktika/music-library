using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MusicLibrary.Models;

public class Genre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long GenreId { get; set; }
    [Required]
    [StringLength(50)]
    public required string Tag { get; set; }
    
    public ICollection<MusicGenre> MusicGenres { get; set; } = new List<MusicGenre>();
}