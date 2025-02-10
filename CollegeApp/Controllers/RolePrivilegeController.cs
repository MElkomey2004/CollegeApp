using AutoMapper;
using CollegeApp.Data.Repository;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolePrivilegeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
		private APIResponse _apiResponse;
		public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
		{
			_mapper = mapper;
			_rolePrivilegeRepository = rolePrivilegeRepository;
			_apiResponse = new();
		}
		[HttpPost]
		[Route("Create")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<APIResponse>> CreateRolePrivilegeAsync(RolePrivilegeDTO dto)
		{
			try
			{
				if (dto == null)
					return BadRequest();

				RolePrivilege rolePrivilege = _mapper.Map<RolePrivilege>(dto);
				rolePrivilege.IsDeleted = false;
				rolePrivilege.CreatedData = DateTime.Now;
				rolePrivilege.ModifiedData = DateTime.Now;

				var result =  _rolePrivilegeRepository.Create(rolePrivilege);

				dto.Id = result.Id;
				_apiResponse.data = dto;
				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;

				//return Ok(_apiResponse);
				return CreatedAtRoute("GetRolePrivilegeById", new { id = dto.Id }, _apiResponse);
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
		[Route("All", Name = "GetAllRolePrivileges")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<APIResponse>  GetRolePrivilegesAsync()
		{
			try
			{
				var rolePrivileges =  _rolePrivilegeRepository.GetAll();

				_apiResponse.data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
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
		[Route("AllRolePrivilegesByRoleId", Name = "GetAllRolePrivilegesByRoleId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public ActionResult<APIResponse> GetRolePrivilegesByRoleId(int roleId)
		{
			try
			{
				var rolePrivileges =  _rolePrivilegeRepository.GetAllByFilter(rolePrivilege => rolePrivilege.RoleId == roleId);

				_apiResponse.data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
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
		[Route("{id:int}", Name = "GetRolePrivilegeById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<APIResponse> GetRolePrivileges(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();

				var rolePrivilege =  _rolePrivilegeRepository.Get(rolePrivilege => rolePrivilege.Id == id);

				if (rolePrivilege == null)
					return NotFound($"The Role Privilege not found with id: {id}");

				_apiResponse.data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
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
		[Route("{name:alpha}", Name = "GetRolePrivilegeByName")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<APIResponse> GetRolePrivileges(string name)
		{
			try
			{
				if (string.IsNullOrEmpty(name))
					return BadRequest();

				var rolePrivilege =  _rolePrivilegeRepository.Get(role => role.RolePrivilegeName.Contains(name));

				if (rolePrivilege == null)
					return NotFound($"The Role Privilege not found with name: {name}");

				_apiResponse.data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
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
		public ActionResult<APIResponse> UpdateRolePrivilege(RolePrivilegeDTO dto)
		{
			try
			{
				if (dto == null || dto.Id <= 0)
					return BadRequest();

				var existingRolePrivilege =  _rolePrivilegeRepository.Get(role => role.Id == dto.Id, true);

				if (existingRolePrivilege == null)
					return BadRequest($"Role Privilege not found with id: {dto.Id} to update");

				var newRolePrivilege = _mapper.Map<RolePrivilege>(dto);

				 _rolePrivilegeRepository.Update(newRolePrivilege);

				_apiResponse.Status = true;
				_apiResponse.StatusCode = HttpStatusCode.OK;
				_apiResponse.data = newRolePrivilege;

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
		[Route("Delete/{id}", Name = "DeleteRolePrivilegeById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public  ActionResult<APIResponse> DeleteRolePrivilege(int id)
		{
			try
			{
				if (id <= 0)
					return BadRequest();

				var rolePrivilege =  _rolePrivilegeRepository.Get(rolePrivilege => rolePrivilege.Id == id);

				if (rolePrivilege == null)
					return BadRequest($"Role Privilege not found with id: {id} to delete");

				 _rolePrivilegeRepository.Delete(rolePrivilege);
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