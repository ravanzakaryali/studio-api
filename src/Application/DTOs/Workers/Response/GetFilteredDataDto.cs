using System;
namespace Space.Application.DTOs.Workers.Response
{
    public class GetFilteredDataDto
    {
        public int Id { get; set; }
        public string? EMail { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public dynamic? Roles { get; set; }
        public List<GetFilteredSessionDto> Sessions { get; set; } = new List<GetFilteredSessionDto>();

    }

    public class GetFilteredSessionDto
    {
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public List<GetFilteredDataClassDto> GetFilteredDataClasses { get; set; } = new List<GetFilteredDataClassDto>();
    }



    public class GetFilteredDataClassDto
    {
        public string ClassName { get; set; } = null!;
        public int ClassId { get; set; }
        public bool IsOpen { get; set; }
        //public string ModuleName { get; set; }
        public string? ModulName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public string ProgramName { get; set; } = null!;
        public int ProgramId { get; set; } 
    }

    

    




}

