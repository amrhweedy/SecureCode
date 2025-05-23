﻿using Ganss.Xss;
using System.ComponentModel.DataAnnotations;

namespace InputValidation.Models;

//public class Student 
//{
//    public int Id { get; set; }

//    public string Name { get; set; }

//    public bool IsActive { get; set; }
//}

// if i send empty object {} => it will give me error because the name is required (because of the [ApiController]) and the id will be 0 and active will be false 
// if i need to make the Name nullable =>     public string? Name { get; set; } and the response will be like this => id =0 , name = null , isActive = false
// if i make the   public string Name { get; set; }= string.Empty;  and send empty json object the response will be like this => id =0 , name = "" , isActive = false , it put default value for the name if it is null




public class Student
{

    private string address;

    public int Id { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "First Name must contain only letters")]
    [Display(Name = "First Name")]
    [StringLength(25, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Last Name must contain only letters")]
    [Display(Name = "Last Name")]
    //[StringLength(25, MinimumLength = 3)]
    [Length(3, 25)]
    public string LastName { get; set; } = string.Empty;


    [AllowedValues("Male", "Female", ErrorMessage = "Only 'Male' and 'Female' are allowed")]
    public string Gender { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;


    public string Address
    {
        get => address;
        set => address = new HtmlSanitizer().Sanitize(value); // html sanitization => remove the harmful html tags like <script> to avoid xss attacks (notion => SecureCoding)
    }
}