using System;
namespace Space.Application.DTOs.Workers.Response
{
    public class GetAllSurveyDto
    {
        public string ClassName { get; set; } = null!;
        public int ClassId { get; set; }
        public string? RoomName { get; set; }
        public int? RoomId { get; set; }
        public bool IsOpen { get; set; }
        public string? ProgramName { get; set; }


        public List<GetAllSurveyModulesDto>? Modules { get; set; } = new List<GetAllSurveyModulesDto>();


    }

    public class GetAllSurveyModulesDto
    {
        public int? ModuleId { get; set; }
        public string? ModulName { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        //public bool? IsExam { get; set; }
        public bool IsSurvey { get; set; }

    }

}

