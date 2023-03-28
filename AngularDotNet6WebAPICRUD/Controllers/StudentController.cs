using AngularDotNet6WebAPICRUD.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularDotNet6WebAPICRUD.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _studentDbContext;

        public StudentController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IEnumerable<Student>> GetStudents()
        {

            return await _studentDbContext.Students.ToListAsync();
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<Student> AddStudent(Student objStudent)
        {

            _studentDbContext.Students.Add(objStudent);
            await _studentDbContext.SaveChangesAsync();
            return objStudent;
        }

        [HttpPatch]
        [Route("UpdateStudent/{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(Student objStudent)
        {
            var studentExist = await _studentDbContext.Students.FindAsync(objStudent.Id);

            if (studentExist == null)
                return BadRequest("Student not found!");

            studentExist.Name = objStudent.Name;
            studentExist.Course = objStudent.Course;
            studentExist.Id = objStudent.Id;
            await _studentDbContext.SaveChangesAsync();

            return Ok(await _studentDbContext.Students.ToListAsync());
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public bool DeleteStudent(int id)
        {

            bool a = false;
            var student = _studentDbContext.Students.Find(id);
            if (student != null)
            {
                a = true;
                _studentDbContext.Entry(student).State = EntityState.Deleted;
                _studentDbContext.SaveChanges();
            }

            return a;
        }
    }
}
