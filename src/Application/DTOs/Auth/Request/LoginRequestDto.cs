﻿namespace Space.Application.DTOs;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    //public string Token { get; set; } = null!;
}
