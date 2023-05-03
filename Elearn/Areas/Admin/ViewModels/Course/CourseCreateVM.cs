using System;
using System.ComponentModel.DataAnnotations;
using Elearn.Models;

namespace Elearn.Areas.Admin.ViewModels.Course
{
	public class CourseCreateVM
	{
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public int Sales { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}