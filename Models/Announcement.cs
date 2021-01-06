using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsApp.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Category Category { get; set; }
        public DateTime AddedDate { get; set; }
        public String Author { get; set; }
        public String City { get; set; }
        public String Mail { get; set; }
        public String Telephone { get; set; }


    }
}
