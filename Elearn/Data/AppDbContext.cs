using System;
using Elearn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elearn.Data
{
	public class AppDbContext: IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

		public DbSet<Author> Authors { get; set; }
		public DbSet<Course> Course { get; set; }
		public DbSet<CourseImage> CourseImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasQueryFilter(a => !a.SoftDelete);
            modelBuilder.Entity<Course>().HasQueryFilter(c => !c.SoftDelete);
            modelBuilder.Entity<CourseImage>().HasQueryFilter(ci => !ci.SoftDelete);

            modelBuilder.Entity<Author>().HasData(
				new Author
				{
					Id = 1,
					FullName = "Mark Smith",
					ProfilePhoto = "course_author_2.jpg"
                },

				new Author
				{
                    Id = 2,
                    FullName = "Julia Williams",
                    ProfilePhoto = "course_author_3.jpg"
                },

                new Author
                {
                    Id = 3,
                    FullName = "James S. Morrison",
                    ProfilePhoto = "featured_author.jpg"
                }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "Social Media Course",
                    Description = "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.",
                    Price = 35,
                    Sales = 352,
                    AuthorId = 1
                },

                new Course
                {
                    Id = 2,
                    Title = "Marketng Course",
                    Description = "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.",
                    Price = 35,
                    Sales = 352,
                    AuthorId = 2
                },

                new Course
                {
                    Id = 3,
                    Title = "Online Literature Course",
                    Description = "Maecenas rutrum viverra sapien sed ferm entum. Morbi tempor odio eget lacus tempus pulvinar.",
                    Price = 35,
                    Sales = 352,
                    AuthorId = 3
                }
            );

            modelBuilder.Entity<CourseImage>().HasData(
                new CourseImage
                {
                    Id = 1,
                    Name = "course_2.jpg",
                    IsMain = true,
                    CourseId = 1
                },

                new CourseImage
                {
                    Id = 2,
                    Name = "course_1.jpg",
                    CourseId = 1
                },

                new CourseImage
                {
                    Id = 3,
                    Name = "course_3.jpg",
                    IsMain = true,
                    CourseId = 2
                },

                new CourseImage
                {
                    Id = 4,
                    Name = "course_2.jpg",
                    CourseId = 2
                },

                new CourseImage
                {
                    Id = 5,
                    Name = "course_1.jpg",
                    IsMain = true,
                    CourseId = 3
                },

                new CourseImage
                {
                    Id = 6,
                    Name = "course_3.jpg",
                    CourseId = 3
                }
            );
        }
    }
}