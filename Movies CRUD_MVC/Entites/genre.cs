using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_CRUD_MVC.Entites
{
    public class genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public byte Id { get; set; }


        [Required]
        [MaxLength(350)]    
        
        public string name { get; set; }

    
    }
}
