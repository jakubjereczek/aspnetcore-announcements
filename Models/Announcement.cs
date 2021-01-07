using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsApp.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        [RegularExpression("[^#!$%^&*~]*")]
        [Required(ErrorMessage = "Musisz wprowadzić tytuł ogłoszenia.")]
        public String Title { get; set; }
        [RegularExpression("[^#!$%^&*~]*")]
        [Required(ErrorMessage = "Musisz wprowadzić opis ogłoszenia.")]
        public String Description { get; set; }
        [Required(ErrorMessage = "Musisz wybrać kategorie ogłoszenia.")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AddedDate { get; set; }
        public String Author { get; set; }
        public String City { get; set; }
        [EmailAddress]
        public String Mail { get; set; }
        [Phone]
        public String Telephone { get; set; }


    }
}
