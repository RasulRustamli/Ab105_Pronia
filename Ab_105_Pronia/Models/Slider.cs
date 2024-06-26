﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ab_105_Pronia.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [StringLength(25,ErrorMessage ="Uzlug 25 i kece bilmez")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string? ImgUrl { get; set; }
        //[NotMapped]
        //public IFormFile ImgFile { get; set; }
    }
}
