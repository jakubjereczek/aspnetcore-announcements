using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { set; get; }
        public String Name { set; get; }
    }
}
