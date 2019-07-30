using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Data;
using ImageGallery.Interface;
using ImageGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.Controllers
{
    public class GalleryController : Controller
    {
            //В объявлении поля readonly указывает на то, что присвоение значения полю может 
            //происходить только при объявлении или в конструкторе этого класса.
            //Полю только для чтения можно несколько раз назначить значения в объявлении 
            //поля и в конструкторе.
            //Поле readonly нельзя изменять после выхода из конструктора.Это влечет за собой разные 
            //последствия для типов значений и ссылочных типов.
            //Поскольку типы значений содержат данные, поле readonly с типом значения является 
            //неизменяемым.
            //Ссылочные типы содержат только ссылку на соответствующие данные, а значит поле 
            //readonly ссылочного типа будет всегда ссылаться на один объект.Но сам этот 
            //объект не является неизменяемым. Модификатор readonly запрещает замену 
            //поля другим экземпляром ссылочного типа.Но этот модификатор не препятствует 
            //изменению данных экземпляра, на которое ссылается поле только для чтения, в том числе через это поле.
        private readonly ApplicationDbContext _context;
        private readonly IUploadInterface _upload;

        public GalleryController(ApplicationDbContext context, IUploadInterface upload)
        {
            _context = context;
            _upload = upload;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //OrderByDescending - эта операции прототипирована и ведет себя подобно 
            //операции OrderBy, но с тем отличием, что упорядочивает по убыванию.
            var getDetailPicture = _context.ImageDetail.Include(i => i.Category)
                .ToList().OrderByDescending(a => a.ReleaseDate);
            //оператор перехода
            return View(getDetailPicture);
        }


        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            var image = from i in _context.ImageDetail.Include(p => p.Category)
            select i;
             
            //оператор выбора
            //если текст не пустой
            if (!String.IsNullOrEmpty(searchString))
            {
                //Операция Where используется для фильтрации элементов
                //Операция Contains возвращает true, если любой элемент 
                //входной последовательности соответствует указанному значению.
                image = image.Where(s => s.ImageName.Contains(searchString));
            }
            //оператор перехода
            return View(await image.ToListAsync());

        }


        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.

        //Отличие int? от простого int в том, что он может принимать еще одно значение - null. 
        //Это удобно, например, при извлечении данных из базы, где значения могут быть неопределенными.
        public async Task<IActionResult> Details(int? id)
        {
            //оператор выбора
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail
                .Include(p => p.Category)
                //FirstOrDefaultAsync: получение первого элемента или значения по умолчанию
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pictureDetail == null)
            {
                return NotFound();
            }

            return View(pictureDetail);
        }

        
    }
}