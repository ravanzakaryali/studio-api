using System;
namespace Space.Application.DTOs
{
	public class GetExamSheetDto
	{
		public int ModuleId { get; set; }
		public int ClassId { get; set; }
		public bool IsChecked { get; set; }
	}
}

