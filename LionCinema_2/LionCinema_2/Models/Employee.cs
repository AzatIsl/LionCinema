using System;
using System.Collections.Generic;

namespace LionCinema_2.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeLog { get; set; } = null!;

    public string EmployeePas { get; set; } = null!;
}
