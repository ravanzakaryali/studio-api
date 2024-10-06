
using System;
namespace Space.Domain.Entities;

public class ExamSheet :BaseAuditableEntity
{
	public Module Module { get; set; }
	public int ModuleId { get; set; }

	public Class Class { get; set; }
	public int ClassId { get; set; }

}

