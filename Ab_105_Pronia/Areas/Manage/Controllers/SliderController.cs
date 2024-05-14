using Ab_105_Pronia.DAL;
using Ab_105_Pronia.Models;
using Ab_105_Pronia.ViewModels.Slider;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Ab_105_Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController:Controller 
    {

        AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
           _environment = environment;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();
            return View(sliders);
        }
      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSliderVm slidervm)
        {

            if(!slidervm.ImgFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImgFile", "Duzgun format daxil edin");
                return View();
            }
            if(slidervm.ImgFile.Length > 2097152)
            {
                ModelState.AddModelError("ImgFile", "Uzunluq max 2 mb ola biler");
                return View();
            }
            //string path= @"C:\Users\rasul\OneDrive\Masaüstü\AB105_WEBAPP\Ab_105_Pronia\Ab_105_Pronia\wwwroot\Upload\Slider\";
            string path = _environment.WebRootPath + @"\Upload\Slider\";
            string filename=Guid.NewGuid()+slidervm.ImgFile.FileName;


            using(FileStream stream=new FileStream(path+filename,FileMode.Create))
            {
                slidervm.ImgFile.CopyTo(stream);
            }









            if (!ModelState.IsValid)
            {
                return View();
            }
            Slider slider = new Slider()
            {
                Title = slidervm.Title,
                SubTitle = slidervm.SubTitle,
                Description = slidervm.Description,
                ImgUrl = filename,
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(x=>x.Id==id);

            if (slider != null)
            {
                string path = _environment.WebRootPath + @"\Upload\Slider\" + slider.ImgUrl;
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                _context.Sliders.Remove(slider);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
        public IActionResult Update(int id)
        {
            Slider slider=_context.Sliders.FirstOrDefault(X=>X.Id==id);
            UpdateSliderVm sliderVm = new UpdateSliderVm()
            {
                Id = slider.Id,
                Title = slider.Title,
                SubTitle = slider.SubTitle,
                Description = slider.Description,
                ImgUrl = slider.ImgUrl
            };
            if(slider==null)
            {
                return RedirectToAction("Index");   
            }
            return View(sliderVm);
        }
        [HttpPost]
        public IActionResult Update(UpdateSliderVm slider)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var oldSlider=_context.Sliders.FirstOrDefault(x=>x.Id == slider.Id);
            if(oldSlider==null) {return RedirectToAction("Index"); }
            oldSlider.Title = slider.Title;
            oldSlider.SubTitle = slider.SubTitle;
            oldSlider.ImgUrl = slider.ImgUrl;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
