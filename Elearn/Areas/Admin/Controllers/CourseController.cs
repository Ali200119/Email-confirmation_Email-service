using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearn.Areas.Admin.ViewModels.Course;
using Elearn.Data;
using Elearn.Helpers;
using Elearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _context.Course.Include(c => c.CourseImages).ToListAsync();

            List<CourseListVM> model = new List<CourseListVM>();

            foreach (var course in courses)
            {
                CourseListVM mappedData = new CourseListVM
                {
                    Id = course.Id,
                    Title = course.Title,
                    Price = course.Price,
                    Sales = course.Sales,
                    Image = course.CourseImages.FirstOrDefault(c => c.IsMain).Name
                };

                model.Add(mappedData);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await GetAuthorsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM model)
        {
            ViewBag.Authors = await GetAuthorsAsync();

            if (!ModelState.IsValid) return View(model);

            foreach(var photo in model.Photos)
            {
                if(!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photos", "File type must be image");
                    return View(model);
                }
            }

            List<CourseImage> courseImages = new List<CourseImage>();

            foreach (var photo in model.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                await photo.CreateLocalFileAsync(path);

                CourseImage courseImage = new CourseImage
                {
                    Name = fileName
                };

                courseImages.Add(courseImage);
            }

            courseImages.FirstOrDefault().IsMain = true;

            decimal convertedPrice = decimal.Parse(model.Price.Replace(".", ","));

            Course course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Price = convertedPrice,
                Sales = model.Sales,
                AuthorId = model.AuthorId,
                CourseImages = courseImages
            };

            await _context.CourseImages.AddRangeAsync(courseImages);
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();

            Course course = await _context.Course.Include(c => c.CourseImages).Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == id);
            if (course is null) return NotFound();

            List<string> courseImages = new List<string>();

            foreach (var image in course.CourseImages)
            {
                courseImages.Add(image.Name);
            }

            CourseDetailsVM model = new CourseDetailsVM
            {
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Sales = course.Sales,
                CourseImages = courseImages,
                AuthorFullName = course.Author.FullName
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            ViewBag.Authors = await GetAuthorsAsync();

            Course dbCourse = await _context.Course.Include(c => c.CourseImages).Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == id);

            if (dbCourse == null) return NotFound();


            CourseEditVM model = new()
            {
                Id = dbCourse.Id,
                Title = dbCourse.Title,
                Sales = dbCourse.Sales,
                Price = dbCourse.Price.ToString("0.#####"),
                AuthorId = dbCourse.AuthorId,
                Images = dbCourse.CourseImages.ToList(),
                Description = dbCourse.Description
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM updatedCourse)
        {
            if (id == null) return BadRequest();

            ViewBag.Authors = await GetAuthorsAsync();

            Course dbCourse = await _context.Course.AsNoTracking().Include(c => c.CourseImages).Include(c => c.Author).FirstOrDefaultAsync(c => c.Id == id);

            if (dbCourse == null) return NotFound();

            if (!ModelState.IsValid)
            {
                updatedCourse.Images = dbCourse.CourseImages.ToList();
                return View(updatedCourse);
            }

            List<CourseImage> courseImages = new();

            if (updatedCourse.Photos is not null)
            {
                foreach (var photo in updatedCourse.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image.");
                        updatedCourse.Images = dbCourse.CourseImages.ToList();
                        return View(dbCourse);
                    }
                }

                foreach (var photo in updatedCourse.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                    await photo.CreateLocalFileAsync(path);

                    CourseImage courseImage = new()
                    {
                        Name = fileName
                    };

                    courseImages.Add(courseImage);
                }

                await _context.CourseImages.AddRangeAsync(courseImages);
            }

            decimal convertedPrice = decimal.Parse(updatedCourse.Price.Replace(".", ","));

            Course newCourse = new()
            {
                Id = updatedCourse.Id,
                Title = updatedCourse.Title,
                Price = convertedPrice,
                Sales = updatedCourse.Sales,
                Description = updatedCourse.Description,
                AuthorId = updatedCourse.AuthorId,
                CourseImages = courseImages.Count == 0 ? dbCourse.CourseImages : courseImages
            };


            _context.Course.Update(newCourse);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourseImage(int? id)
        {
            if (id == null) return BadRequest();

            bool result = false;

            CourseImage courseImage = await _context.CourseImages.FirstOrDefaultAsync(c => c.Id == id);

            if (courseImage is null) return NotFound();

            Course course = await _context.Course.Include(c => c.CourseImages).FirstOrDefaultAsync(c => c.Id == courseImage.CourseId);

            if (course.CourseImages.Count > 1)
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "images", courseImage.Name);

                FileHelper.DeleteFileFromPath(path);

                _context.CourseImages.Remove(courseImage);

                await _context.SaveChangesAsync();

                result = true;
            }

            course.CourseImages.FirstOrDefault().IsMain = true;

            await _context.SaveChangesAsync();

            return Ok(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            Course course = await _context.Course.Include(c => c.CourseImages).FirstOrDefaultAsync(c => c.Id == id);

            foreach (var image in course.CourseImages)
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "images", image.Name);

                FileHelper.DeleteFileFromPath(path);
            }

            _context.Course.Remove(course);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> GetAuthorsAsync()
        {
            IEnumerable<Author> authors = await _context.Authors.ToListAsync();

            return new SelectList(authors, "Id", "FullName");
        }
    }
}