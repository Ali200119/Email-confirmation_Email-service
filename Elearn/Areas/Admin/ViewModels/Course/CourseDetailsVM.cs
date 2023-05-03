using System;
using Elearn.Models;

namespace Elearn.Areas.Admin.ViewModels.Course
{
	public class CourseDetailsVM
	{
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Sales { get; set; }
        public string AuthorFullName { get; set; }
        public IEnumerable<string> CourseImages { get; set; }
    }
}