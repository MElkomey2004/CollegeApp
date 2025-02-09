using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeApp.Controllers
{

	[Route("api/[controller]")]
	[ApiController] //This is reponsible for validation and other requirements i will add this soon.

	
	//[Authorize(AuthenticationSchemes ="LoginForLocalUsers",Roles ="Superadmin,Admin")]
	public class StudentController : ControllerBase
	{

		private readonly ILogger<StudentController> _logger ;
		private readonly IMapper _mapper;
		private readonly IStudentRepository _studentRepository;
		private APIResponse _apiResponse;
        public StudentController(ILogger<StudentController> logger , IMapper mapper , IStudentRepository studentRepository)
        {
			_mapper = mapper;
			_logger = logger;
			_studentRepository = studentRepository;
			_apiResponse = new();
        }



        [HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//[AllowAnonymous]

		public  ActionResult< IEnumerable<APIResponse>> GetStudents()
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

			try
			{
				_apiResponse.data = _studentRepository.GetAll().Select(s => new StudentDTO()
				{

					Id = s.Id,
					StudentName = s.StudentName,
					Address = s.Address,
					Email = s.Email,
					DOB = s.DOB,

				}).ToList();
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}

		

		}

		[HttpGet("{id:int}" , Name = "GetStudentById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<StudentDTO> Get(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();

				var student = _studentRepository.Get(i => i.Id == id);
				if (student == null)
					return NotFound("The student is not exist");


				_apiResponse.data = new StudentDTO()
				{
					Id = student.Id,
					StudentName = student.StudentName,
					Email = student.Email,
					Address = student.Address
				};

				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}
		
		}

		[HttpGet("Name:string")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<StudentDTO> GetStudnetByName(string Name)
		{
			try
			{
				if (string.IsNullOrEmpty(Name))
					return BadRequest();

				var student = _studentRepository.Get(i => i.StudentName.ToLower().Contains(Name.ToLower()));
				if (student == null)
					return NotFound("This Student is not Exist");

				_apiResponse.data = new StudentDTO()
				{
					Id = student.Id,
					StudentName = student.StudentName,
					Email = student.Email,
					Address = student.Address,
					DOB = student.DOB
				};
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				return Ok(_apiResponse);

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}
		
		}


		[HttpPost]
		[Route("Create")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]

		public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
		{
			try
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
					DOB = model.DOB
				};

				var Record = _studentRepository.Create(student);

				model.Id = Record.Id;

				_apiResponse.data = student;
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				return CreatedAtRoute("GetStudentById", new { id = model.Id }, _apiResponse);

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}

			
		}



		[HttpPut]
		[Route("Update")]
		
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult UpdateStudent([FromBody] StudentDTO model)
		{

			try
			{
				if (model == null || model.Id == 0)
				{

					return BadRequest();
				}

				var existingStudent = _studentRepository.Get(i => i.Id == model.Id, true);
				if (existingStudent == null)
					return NotFound();


				existingStudent.StudentName = model.StudentName;
				existingStudent.Email = model.Email;
				existingStudent.Address = model.Address;
				existingStudent.DOB = model.DOB;

				_studentRepository.Update(existingStudent);

				return NoContent();

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}

		}



		[HttpPut]
		[Route("{id:int}/UpdatePartial")]

		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
		{
			try
			{
				if (patchDocument == null || id == 0)
				{

					return BadRequest();
				}

				var existingStudent = _studentRepository.Get(i => i.Id == id, true);
				if (existingStudent == null)
					return NotFound();


				var StudentDTO = new StudentDTO()
				{
					Id = existingStudent.Id,
					StudentName = existingStudent.StudentName,
					Email = existingStudent.Email,
					Address = existingStudent.Address,
				};

				patchDocument.ApplyTo(StudentDTO, ModelState);


				if (!ModelState.IsValid)
					return BadRequest(ModelState);


				existingStudent.StudentName = StudentDTO.StudentName;
				existingStudent.Email = StudentDTO.Email;
				existingStudent.Address = StudentDTO.Address;
				existingStudent.DOB = StudentDTO.DOB;

				_studentRepository.Update(existingStudent);
				return NoContent();

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}
	

		}

			[HttpDelete("id:int")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<bool>  DeleteStudent(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();
				Student student = _studentRepository.Get(i => i.Id == id);
				if (student == null)
					return NotFound("This Stuent is not already exist");
				_studentRepository.Delete(student);

				_apiResponse.data = true;
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				return Ok(_apiResponse);

			}
			catch (Exception ex)
			{

				_apiResponse.Errors.Add(ex.Message);
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Status = false;
				return Ok(_apiResponse);
			}

		}
	}
}
