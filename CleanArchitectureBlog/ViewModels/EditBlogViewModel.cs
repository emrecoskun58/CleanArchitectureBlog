﻿using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class EditBlogViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(5000, ErrorMessage = "Content cannot be longer than 5000 characters")]
        public string Content { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Order must be a non-negative number")]
        public int Order { get; set; }

        public string UserId { get; set; }

        public IFormFile BlogImage { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set; }
    }
}
