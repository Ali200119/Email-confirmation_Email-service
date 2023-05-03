using System;
using Elearn.Models;

namespace Elearn.Areas.Admin.ViewModels.Course
{
	public class CourseListVM
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Sales { get; set; }
        public string Image { get; set; }
    }
}