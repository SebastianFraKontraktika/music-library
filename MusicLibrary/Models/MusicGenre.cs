using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MusicLibrary.Models;
public class MusicGenre
{
    public long MusicId { get; set; }
    public Music Music { get; set; }
    
    public long GenreId { get; set; }
    public Genre Genre { get; set; }
}
