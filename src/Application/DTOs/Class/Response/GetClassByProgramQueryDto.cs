using System;
namespace Space.Application.DTOs.Class.Response
{
    public class GetClassByProgramQueryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

