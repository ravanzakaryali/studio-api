namespace Space.Application.DTOs;
public class GetClassSurveyDto : GetClassDetailResponse
{
    public DateOnly? StartSurveyDate { get; set; }
    public SurveyStatus SurveyStatus { get; set; }

}