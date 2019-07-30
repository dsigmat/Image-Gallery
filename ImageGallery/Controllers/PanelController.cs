using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Data;
using ImageGallery.Interface;
using ImageGallery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PanelController : Controller
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
        public PanelController(ApplicationDbContext context, IUploadInterface upload)
        {
            _context = context;
            _upload = upload;
        }

        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public IActionResult Index()
        {
            var getDetailPicture = _context.ImageDetail.Include(i => i.Category).ToList().OrderByDescending(a => a.ReleaseDate);
            return View(getDetailPicture);
        }


        [HttpPost]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> Index(string searchString)
        {
            var image = from i in _context.ImageDetail.Include(p => p.Category)
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                image = image.Where(s => s.ImageName.Contains(searchString));
            }

            return View(await image.ToListAsync());

        }


        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public IActionResult Create()
        {
            //ViewData представляет словарь из пар ключ-значение: ключ ["CategoryId"] и значение new SelectList.
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public IActionResult Create(IList<IFormFile> files, ImageDetail image)
        {
            foreach (var item in files)
            {
                image.PathImage = "~/uploads/" + item.FileName.Trim();
                image.Size = item.Length / 1000;
            }
            _upload.UploadFileMultiple(files);
            _context.ImageDetail.Add(image);
            _context.SaveChanges();
            TempData["Sucess"] = "Save Your File";
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", image.CategoryId);
            return RedirectToAction("Create", "Panel");
        }


        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", pictureDetail.CategoryId);
            return View(pictureDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> Edit(int id, ImageDetail imageDetail, IList<IFormFile> files)
        {
            if (id != imageDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in files)
                    {
                        imageDetail.PathImage = "~/uploads/" + item.FileName.Trim();
                        imageDetail.Size = item.Length / 1000;
                    }
                    _upload.UploadFileMultiple(files);
                    _context.ImageDetail.Add(imageDetail);
                    _context.Update(imageDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureDetailExists(imageDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", imageDetail.CategoryId);
            return View(imageDetail);
        }

        private bool PictureDetailExists(int id)
        {
            return _context.ImageDetail.Any(e => e.Id == id);
        }

        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }

            return View(pictureDetail);
        }

        [HttpGet]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }

            return View(pictureDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Объект типа IActionResult, которые непосредственно предназначены для генерации результата действия.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pictureDetail = await _context.ImageDetail.FindAsync(id);
            _context.ImageDetail.Remove(pictureDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}