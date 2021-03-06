﻿using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    /// <summary>
    /// image update model
    /// </summary>
    public class ImageUpdate
    {
        /// <summary>
        /// image id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// is defualt image
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }
    }
}