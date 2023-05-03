using System;
namespace Elearn.Models
{
	public class CourseImage: BaseEntity
	{
		public string Name { get; set; }
		public bool IsMain { get; set; } = false;
		public int CourseId { get; set; }
		public Course Course { get; set; }
	}
}