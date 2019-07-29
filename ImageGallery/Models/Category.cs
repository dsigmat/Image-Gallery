using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class Category
    {
        //данные предоставляются пользователю для заполнения
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
