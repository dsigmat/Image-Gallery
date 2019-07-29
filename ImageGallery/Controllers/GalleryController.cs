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
            var getDetailPicture = _context.ImageDetail.Include(i => i.Category).ToList().OrderByDescending(a => a.ReleaseDate);
            return View(getDetailPicture);
        }


        [HttpPost]
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

        
    }
}