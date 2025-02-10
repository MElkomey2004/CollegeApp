namespace CollegeApp.Data
{
	public class User
	{
		public int Id { get; set; }	
		public string UserName { get; set; }	
		public string Password { get; set; }
		public string PasswordSalt { get;set; }
		public int UserTypeId { get;set; }
		public bool IsActive { get;set; }

		public bool IsDeleted { get; set; }

		public DateTime CreatedData { get;set; }	
		public DateTime ModifiedData { get; set; }

		public virtual UserType UserType { get; set; }	

		public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }


	}
}
