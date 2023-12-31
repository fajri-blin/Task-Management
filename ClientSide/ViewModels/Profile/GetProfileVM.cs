﻿namespace ClientSide.ViewModels.Profile
{
    public class GetProfileVM
    {
        public Guid Guid { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public IFormFile? ImageProfile { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
