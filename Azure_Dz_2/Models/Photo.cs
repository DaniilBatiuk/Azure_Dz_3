﻿namespace Azure_Dz_2.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string PhotoUrl { get; set; } = default!;

        public string Filename { get; set; } = default!;
        public string? Description { get; set; }

    }
}
