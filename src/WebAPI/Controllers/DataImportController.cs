//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Space.Domain.Entities;
//using Space.Domain.Enums;
//using Space.Infrastructure.Persistence;
//using System.Data;
//using System.Data.OleDb;
//using System.Globalization;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace Space.WebAPI.Controllers;
//public class DataImportController : BaseApiController
//{
//    private readonly SpaceDbContext _dbContext;
//    private readonly UserManager<User> _userManager;
//    readonly RoleManager<Role> _roleManager;
//    public DataImportController(UserManager<User> userManager, SpaceDbContext dbContext, RoleManager<Role> roleManager)
//    {
//        _userManager = userManager;
//        _dbContext = dbContext;
//        _roleManager = roleManager;
//    }
//    [HttpPost("worker")]
//    public async Task<IActionResult> ImportWorker([FromBody] ICollection<ImportWorkerDto> workersImport)
//    {
//        List<ImportWorkerDto> importWorkerDtos = new List<ImportWorkerDto>();

//        foreach (var item in workersImport)
//        {
//            if (await _userManager.FindByEmailAsync(item.Email) == null)
//            {
//                importWorkerDtos.Add(item);
//            }
//        }
//        foreach (var item in importWorkerDtos)
//        {
//            Worker worker = new Worker()
//            {
//                Name = item.Name,
//                Surname = item.Surname,
//                IsActive = item.IsActive,
//                Email = item.Email,
//                UserName = item.Email,
//                CreatedBy = "robot"
//            };
//            await _userManager.CreateAsync(worker);
//        }

//        await _dbContext.SaveChangesAsync();
//        return Ok();
//    }

//    [HttpPost("program-modules")]
//    public async Task<IActionResult> ImportProgramAndModules([FromBody] List<ModuleImportDto> modulesImport)
//    {
//        Domain.Entities.Program? program = await _dbContext.Programs.Include(c => c.Modules).ThenInclude(c => c.SubModules).FirstOrDefaultAsync(c => c.Name.Trim() == "Sistem və Bulud Texnologiyaları");
//        List<Module> modules = new List<Module>();
//        modulesImport.ForEach(c =>
//        {
//            Module module = new()
//            {
//                Id = Guid.NewGuid(),
//                Hours = c.Hour,
//                Name = c.Name,
//                Version = c.Version.ToString(),
//                ProgramId = program.Id,

//            };
//            module.SubModules.AddRange(c.Programs.Select(p => new Module()
//            {
//                Name = p.Name,
//                Hours = p.Hour,
//                Version = p.Version.ToString(),
//                TopModuleId = module.Id,
//                ProgramId = program.Id
//            }));
//            modules.Add(module);
//        });
//        await _dbContext.Modules.AddRangeAsync(modules);
//        await _dbContext.SaveChangesAsync();
//        return Ok();
//    }


//    [HttpPost("attendances")]
//    public async Task<IActionResult> Import(AttendanceImport request)
//    {
//        Class? @class = await _dbContext.Classes
//                            .Include(c => c.Session)
//                            .Include(c => c.Studies)
//                            .ThenInclude(c => c.Student)
//                            .ThenInclude(c => c.Contact)
//                            .Include(c => c.ClassSessions)
//                            .ThenInclude(c => c.Attendances)
//                            .FirstOrDefaultAsync(c => c.Name.ToUpper().Contains(request.Name.ToUpper()));

//        List<Worker> workers = await _dbContext.Instrcutors.ToListAsync();
//        if (@class is null)
//        {
//            throw new NotFoundException("Class", request.Name);
//        }
//        if (@class.ClassSessions.Count != 0)
//        {
//            foreach (StudyDto study in request.Studies.Where(c => c.Status.Trim() == "Tələbə"))
//            {
//                Study? studyDb = @class.Studies.FirstOrDefault(c =>
//                      $"{RemoveDiacritics(c.Student?.Contact?.Name)?.ToLowerInvariant().Trim().Replace(" ", "")}{RemoveDiacritics(c.Student?.Contact?.Surname)?.ToLowerInvariant().Trim().Replace(" ", "")}"
//                        .Contains(RemoveDiacritics(study.FullName)?.ToLowerInvariant().Trim().Replace(" ", "")));


//                if (studyDb is null)
//                {
//                    Console.WriteLine(study.FullName.ToLowerInvariant().Trim().ReplaceWhitespace(""));
//                }
//                else
//                {
//                    foreach (AttendanceDto attendance in study.Attendances)
//                    {
//                        List<ClassSession> classSessions = @class.ClassSessions
//                            .Where(c => c.Date == attendance.Date && c.Date <= new DateTime(2023, 06, 06)).ToList();
//                        if (classSessions.Count != 0)
//                        {

//                            foreach (ClassSession classSession in classSessions)
//                            {
//                                classSession.Status = ClassSessionStatus.Offline;
//                                classSession.Attendances.Add(new Attendance()
//                                {
//                                    ClassSessionId = classSession.Id,
//                                    CreatedBy = "Robot",
//                                    StudyId = studyDb.Id,
//                                    TotalAttendanceHours = GetAttendanceValue(attendance.Value, classSession.TotalHour),
//                                });
//                            }
//                        }
//                        else
//                        {
//                            Console.WriteLine("Class session date not found");
//                        }
//                    }
//                }
//            }
//            foreach (StudyDto study in request.Studies.Where(c => c.Status.Trim() == "Müəllim" || c.Status.Trim() == "Mentor"))
//            {
//                Worker? workerDb = workers.FirstOrDefault(c => string.Concat(c.Name, c.Surname).ReplaceWhitespace("").ReplaceAz().RemoveDiacritics().Trim().ToLowerInvariant().Contains(study.FullName.ReplaceAz().RemoveDiacritics().Trim().ToLowerInvariant().ReplaceWhitespace("")));

//                if (workerDb is null)
//                {
//                    Console.WriteLine(study.FullName.RemoveDiacritics().ReplaceAz().ToLower().Trim().ReplaceWhitespace(""));
//                }
//                else
//                {
//                    foreach (AttendanceDto attendance in study.Attendances)
//                    {
//                        List<ClassSession> classSessions = @class.ClassSessions
//                            .Where(c => c.Date == attendance.Date && c.Date <= new DateTime(2023, 06, 06)).ToList();
//                        if (classSessions.Count != 0)
//                        {
//                            if (study.Status.Trim() == "Müəllim")
//                            {
//                                ClassSession? classSession = classSessions.Where(c => c.Category == ClassSessionCategory.Theoric).FirstOrDefault();
//                                if (classSession is not null)
//                                {
//                                    classSession.WorkerId = workerDb.Id;
//                                }
//                                else
//                                {
//                                    Console.WriteLine("Class session category not found");
//                                }
//                            }
//                            if (study.Status.Trim() == "Mentor")
//                            {
//                                ClassSession? classSession = classSessions.Where(c => c.Category == ClassSessionCategory.Practice).FirstOrDefault();
//                                if (classSession is not null)
//                                {
//                                    classSession.WorkerId = workerDb.Id;
//                                }
//                                else
//                                {
//                                    Console.WriteLine("Class session category not found");
//                                }
//                            }
//                        }
//                        else
//                        {
//                            Console.WriteLine("Class session date not found");
//                        }
//                    }
//                }
//            }


//        }
//        else
//        {
//            Console.WriteLine("Class not genrate session");
//        }
//        //await _dbContext.SaveChangesAsync();
//        return Ok();
//    }


//    [NonAction]
//    private int GetAttendanceValue(string value, int totalHour)
//    {
//        switch (value.ToLower())
//        {
//            case "d":
//                return totalHour;
//            case "i":
//                return totalHour;
//            case "y":
//                return 0;
//            case "g":
//                return 0;
//            case "e":
//                return 0;
//            default:
//                return 0;
//        }
//    }
//    [NonAction]
//    string RemoveDiacritics(string input)
//    {
//        string normalizedString = input.Normalize(NormalizationForm.FormD);
//        StringBuilder stringBuilder = new StringBuilder();

//        foreach (var c in normalizedString)
//        {
//            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
//            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
//            {
//                stringBuilder.Append(c);
//            }
//        }

//        return stringBuilder.ToString();
//    }

//    //public async Task<IActionResult> ExcelImport()
//    //{

//    //    //file upload save
//    //    //file get path
//    //    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Asus VivoBook\\Downloads\\RMAD101.xlsx;Extended Properties='Excel 12.0;HDR=YES;'";
//    //    using (OleDbConnection connection = new OleDbConnection(connectionString))
//    //    {


//    //        connection.Open();
//    //        DataTable sheetsTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

//    //        List<string> sheetNames = new List<string>();

//    //        foreach (DataRow row in sheetsTable.Rows)
//    //        {
//    //            string sheetName = row["TABLE_NAME"].ToString();

//    //            sheetNames.Add(sheetName);
//    //        }

//    //        DataTable dataTableInfo = new DataTable();

//    //        OleDbDataAdapter adapterInfo = new OleDbDataAdapter($"SELECT * FROM [{sheetNames[1]}]", connection);
//    //        adapterInfo.Fill(dataTableInfo);

//    //        string dbClassName = "";
//    //        string roomName = "";
//    //        string startDate = "";
//    //        List<string> teacherName = new List<string>();
//    //        foreach (DataRow row in dataTableInfo.Rows)
//    //        {
//    //            var rowClassNameValues = row.ItemArray.FirstOrDefault(c => c.ToString() == "Dərs kodu");
//    //            if (rowClassNameValues != null)
//    //            {
//    //                var index = Array.IndexOf(row.ItemArray, "Dərs kodu");
//    //                dbClassName = row.ItemArray[index + 1].ToString();
//    //            }
//    //            var rowValues = row.ItemArray.FirstOrDefault(c => c.ToString() == "Dərs otağı");
//    //            if (rowValues != null)
//    //            {
//    //                var index = Array.IndexOf(row.ItemArray, "Dərs otağı");
//    //                roomName = row.ItemArray[index + 1].ToString();
//    //            }
//    //            //regex boşluq
//    //            var rowTeahcerValue = row.ItemArray.Where(row => Regex.IsMatch(row.ToString(), @"Müəllim\d+"));
//    //            foreach (var item in rowTeahcerValue)
//    //            {
//    //                var index = Array.IndexOf(row.ItemArray, item);
//    //                var value = row.ItemArray[index + 1].ToString();
//    //                if (!string.IsNullOrEmpty(value))
//    //                    teacherName.Add(value);
//    //            }
//    //            var rowMentorValue = row.ItemArray.Where(row => Regex.IsMatch(row.ToString(), @"Mener\d+"));
//    //            foreach (var item in rowMentorValue)
//    //            {
//    //                var index = Array.IndexOf(row.ItemArray, item);
//    //                var value = row.ItemArray[index + 1].ToString();
//    //                if (!string.IsNullOrEmpty(value))
//    //                    teacherName.Add(value);
//    //            }
//    //            var rowStartDateValues = row.ItemArray.FirstOrDefault(c => c.ToString() == "Başlama tarixi");
//    //            if (rowStartDateValues != null)
//    //            {
//    //                var index = Array.IndexOf(row.ItemArray, "Başlama tarixi");
//    //                startDate = row.ItemArray[index + 1].ToString();
//    //            }

//    //        }
//    //        Console.WriteLine(teacherName);

//    //    }

//    //    return NoContent();
//    //}
//}


//public static class StringExt
//{

//    private static readonly Regex sWhitespace = new Regex(@"\s+");
//    public static string ReplaceWhitespace(this string input, string replacement)
//    {
//        return sWhitespace.Replace(input, replacement);
//    }
//    public static string ReplaceAz(this string name)
//    {
//        int i = name.IndexOfAny(new char[] { 'ş', 'ö', 'ğ', 'ü', 'ı', 'ə' });
//        string newName = name.ToLower();
//        if (i > -1)
//        {
//            StringBuilder outPut = new(newName);
//            outPut.Replace('ö', 'o');
//            outPut.Replace('ş', 's');
//            outPut.Replace('ı', 'i');
//            outPut.Replace('ğ', 'g');
//            outPut.Replace('ü', 'u');
//            outPut.Replace('ə', 'a');
//            newName = outPut.ToString();
//        }
//        return newName.Replace("x", "kh").Replace("ç", "ch");
//    }
//    public static string RemoveDiacritics(this string input)
//    {
//        string normalizedString = input.Normalize(NormalizationForm.FormD);
//        StringBuilder stringBuilder = new StringBuilder();

//        foreach (var c in normalizedString)
//        {
//            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
//            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
//            {
//                stringBuilder.Append(c);
//            }
//        }

//        return stringBuilder.ToString();
//    }
//}
