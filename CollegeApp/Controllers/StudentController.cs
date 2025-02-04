using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{

	[Route("api/[controller]")]
	[ApiController] //This is reponsible for validation and other requirements i will add this soon.
	public class StudentController : ControllerBase
	{

		private readonly ILogger<StudentController> _logger ;
		private readonly IMapper _mapper;
        public StudentController(ILogger<StudentController> logger , IMapper mapper)
        {
			_mapper = mapper;
			_logger = logger;
        }



        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public  ActionResult< IEnumerable<StudentDTO>> GetStudents()
		{
			//var Students = new List<StudentDTO>();


			//foreach (var item in CollegeRepository.Students)
			//{
			//	StudentDTO obj = new StudentDTO
			//	{
			//		Id = item.Id,
			//		StudentName = item.StudentName,
			//		Address = item.Address,
			//		Email = item.Email,
			//	};

			//	Students.Add(obj);
			//}

			//var Students = _dbContext.Students;


			var Students =  _dbContext.Students.Select(s => new StudentDTO()
			{

				Id = s.Id,
				StudentName = s.StudentName,
				Address = s.Address,
				Email = s.Email,
				DOB = s.DOB,

			}).ToList();

			return Ok(Students);

		}

		[HttpGet("{id:int}" , Name = "GetStudentById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<StudentDTO> GetStudentById(int id)
		{
			if (id <= 0)
				return BadRequest();
			
			var student = _dbContext.Students.FirstOrDefault(i => i.Id == id);
			if (student == null)
				return NotFound("The student is not exist");


			var StudentDTO = new StudentDTO()
			{
				Id =student.Id,
				StudentName = student.StudentName,
				Email = student.Email,
				Address = student.Address
			};
			return Ok(StudentDTO);
		}

		[HttpGet("Name:string")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult <StudentDTO> GetStudnetByName(string Name)
		{
			if (string.IsNullOrEmpty(Name))
				return BadRequest();

			var student = _dbContext.Students.FirstOrDefault(s => s.StudentName == Name);
			if (student == null)
				return NotFound("This Student is not Exist");

			var StudentDTO = new StudentDTO()
			{
				Id = student.Id,
				StudentName = student.StudentName,
				Email = student.Email,
				Address = student.Address,
				DOB = student.DOB
			};
			return Ok(StudentDTO);
		}


		[HttpPost]
		[Route("Create")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
		{

			if (model == null)
				return BadRequest();

			//if (model.AdmissionDate < DateTime.Now)
			//{
			//	ModelState.AddModelError("AddmissionDate Error", "Admission Date must be greater than or equal to todays date");
			//	return BadRequest(ModelState);
			//}



			var student = new Student()
			{
				StudentName = model.StudentName,
				Email = model.Email,
				Address = model.Address,
				DOB=model.DOB
			};

			_dbContext.Students.Add(student);
			_dbContext.SaveChanges();

			model.Id = student.Id;

			return CreatedAtRoute("GetStudentById" , new {id = model.Id} , model);
			
		}



		[HttpPut]
		[Route("Update")]
		
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult UpdateStudent([FromBody] StudentDTO model)
		{
			if (model == null || model.Id == 0)
			{

				return BadRequest();
			}

			var existingStudent = _dbContext.Students.FirstOrDefault(s => s.Id == model.Id);
			if (existingStudent == null)
				return NotFound();

			
			existingStudent.StudentName = model.StudentName;
			existingStudent.Email = model.Email;
			existingStudent.Address = model.Address;
			existingStudent.DOB = model.DOB;
			_dbContext.SaveChanges();

			return NoContent();
		}



		[HttpPut]
		[Route("{id:int}/UpdatePartial")]

		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
		{
			if (patchDocument == null || id == 0)
			{

				return BadRequest();
			}

			var existingStudent = _dbContext.Students.FirstOrDefault(s => s.Id ==id);
			if (existingStudent == null)
				return NotFound();


			var StudentDTO = new StudentDTO()
			{
				Id = existingStudent.Id,
				StudentName = existingStudent.StudentName,
				Email = existingStudent.Email,
				Address = existingStudent.Address,
			};

			patchDocument.ApplyTo(StudentDTO , ModelState);


			if (!ModelState.IsValid)
				return BadRequest(ModelState);


			existingStudent.StudentName = StudentDTO.StudentName;
			existingStudent.Email = StudentDTO.Email;
			existingStudent.Address = StudentDTO.Address;
			existingStudent.DOB = StudentDTO.DOB;

			_dbContext.SaveChanges();
			return NoContent();

		}

			[HttpDelete("id:int")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<bool>  DeleteStudent(int id)
		{
			if (id <= 0)
				return BadRequest();
			Student student = _dbContext.Students.FirstOrDefault(n => n.Id == id);
			if (student == null)
				return NotFound("This Stuent is not already exist");
			_dbContext.Students.Remove(student);

			_dbContext.SaveChanges();

			return Ok(true);
		}
	}
}
