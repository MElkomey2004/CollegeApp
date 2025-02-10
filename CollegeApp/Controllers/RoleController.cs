using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICollegeRepository<Role> _roleRepository;
		private APIResponse _apiResponse;
		public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
		{
			_mapper = mapper;
			_roleRepository = roleRepository;
			_apiResponse = new();
		}

		[HttpPost]
		[Route("Create")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public  ActionResult<APIResponse> CreateRole(RoleDTO dto)
		{
			try
			{
				if (dto == null)
					return BadRequest();

				Role role = _mapper.Map<Role>(dto);
				role.IsDeleted = false;
				role.CreatedData = DateTime.Now;
				role.ModifiedData = DateTime.Now;

				var result =  _roleRepository.Create(role);

				dto.Id = result.Id;
				_apiResponse.data = dto;
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);
				//return CreatedAtRoute("GetRoleById", new { id = dto.Id }, _apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}

		[HttpGet]
		[Route("All", Name = "GetAllRoles")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public  ActionResult<APIResponse> GetRoles()
		{
			try
			{
				var roles =  _roleRepository.GetAll();

				_apiResponse.data = _mapper.Map<List<RoleDTO>>(roles);
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}

		[HttpGet]
		[Route("{id:int}", Name = "GetRoleById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public  ActionResult<APIResponse> GetRoles(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();

				var role =  _roleRepository.Get(role => role.Id == id);

				if (role == null)
					return NotFound($"The Role not found with id: {id}");

				_apiResponse.data = _mapper.Map<RoleDTO>(role);
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}

		[HttpGet]
		[Route("{name:alpha}", Name = "GetRoleByName")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<APIResponse> GetRoles(string name)
		{
			try
			{
				if (string.IsNullOrEmpty(name))
					return BadRequest();

				var role =  _roleRepository.Get(role => role.RoleName == name);

				if (role == null)
					return NotFound($"The Role not found with name: {name}");

				_apiResponse.data = _mapper.Map<RoleDTO>(role);
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}

		[HttpPut]
		[Route("Update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<APIResponse> UpdateRole(RoleDTO dto)
		{
			try
			{
				if (dto == null || dto.Id <= 0)
					return BadRequest();

				var existingRole =  _roleRepository.Get(role => role.Id == dto.Id, true);

				if (existingRole == null)
					return BadRequest($"Role not found with id: {dto.Id} to update");

				var newRole = _mapper.Map<Role>(dto);

				 _roleRepository.Update(newRole);

				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				_apiResponse.data = newRole;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}

		[HttpDelete]
		[Route("Delete/{id}", Name = "DeleteRoleById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<APIResponse> DeleteRole(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();

				var role =  _roleRepository.Get(role => role.Id == id);

				if (role == null)
					return BadRequest($"Role not found with id: {id} to delete");

				 _roleRepository.Delete(role);
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				_apiResponse.data = true;

				return Ok(_apiResponse);
			}
			catch (Exception ex)
			{
				_apiResponse.Status = false;
				_apiResponse.StatusCode = HttpStatusCode.InternalServerError;
				_apiResponse.Errors.Add(ex.Message);
				return _apiResponse;
			}
		}
	}
}