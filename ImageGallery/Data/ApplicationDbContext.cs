using ImageGallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){}

        //Для каждого класса модели данных, к которому нужен доступ, в классе контекста 
        //определено свойство, возвращающее объект DbSet<T> - именно через него сохраняются 
        //и извлекаются данные.Поскольку определено свойство, которое возвращает объект
        //DbSet<ImageDetail>, можно сохранять и извлекать объекты ImageDetail. 
        public DbSet<ImageDetail> ImageDetail { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
