using System;
using System.ComponentModel.DataAnnotations;
using Elearn.Models;

namespace Elearn.Areas.Admin.ViewModels.Course
{
	public class CourseEditVM
	{
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public int Sales { get; set; }

        public int AuthorId { get; set; }

        public List<IFormFile>? Photos { get; set; }

        public List<CourseImage>? Images { get; set; }
    }
}