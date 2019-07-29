using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class ImageDetail
    {
        //данные предоставляются пользователю для заполнения
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string ImageName { get; set; }

        [Display(Name = "Description")]
        public string ImageDescription { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Image")]
        public string PathImage { get; set; } = null;

        [Display(Name = "Size")]
        public long Size { get; set; }

        //внешний ключ
        public int CategoryId { get; set; }
        //навигационное свойство 
        public Category Category { get; set; }
    }
}
