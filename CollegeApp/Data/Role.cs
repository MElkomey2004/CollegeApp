namespace CollegeApp.Data
{
	public class Role
	{
		public int Id { get; set; }	
		public string RoleName { get;set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }

		public bool IsDeleted { get; set; }
		public DateTime CreatedData { get; set; }
		public DateTime ModifiedData { get; set; }

		public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }
		public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }

	}
}
