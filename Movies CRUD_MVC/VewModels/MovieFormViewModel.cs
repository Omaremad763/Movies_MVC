using Movies_CRUD_MVC.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_CRUD_MVC.VewModels
{
    public class MovieFormViewModel
    {
        
        public int Id { get; set; }
        
        [StringLength(400)]
        public string Title { get; set; }
        public int year { get; set; }

        [Range(1,10)]
        public double rate { get; set; }


        [Required, StringLength(1000)]
        public string Storyline { get; set; }

        
        public byte[] Poster { get; set; }

       

        [Display(Name ="Genre")]
        public byte GenreId { get; set; }
    
        public IEnumerable<genre>Genres { get; set; }
    }
}
