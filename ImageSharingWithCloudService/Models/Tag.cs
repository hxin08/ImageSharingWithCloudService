using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageSharingWithCloudService.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public String Name { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}