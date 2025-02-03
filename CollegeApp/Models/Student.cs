﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Model
{
	public class Student
	{
		public int Id { get; set; }
		public string StudentName { get; set; }	
		public string Address { get; set; }
		public string Email { get; set; }


	}
}
