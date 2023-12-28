using System;
using System.Collections.Generic;

namespace orkesterapp.Models;
public class User
{
    public int ID { get; set; }
    public string? LastName { get; set; }
    public string? FirstMidName { get; set; }
    public string Email { get; set; }
    public string Geslo { get; set; }

    public int? RoleID  { get; set; }
    public int? OrchesterID  { get; set; }
}