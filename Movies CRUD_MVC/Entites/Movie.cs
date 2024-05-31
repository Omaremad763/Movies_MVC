using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_CRUD_MVC.Entites
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        public string Title { get; set; }
        public int year { get; set; }
        public double rate { get; set; }
        
       
        [Required,MaxLength(1000)]
        public string Storyline { get; set; }
        
        [Required]
        public byte[] Poster { get; set; }

       
        public byte GenreId { get; set; }

        public genre Genre { get; set; }




    }
}
