using System;
using System.Collections.Generic;

namespace LionCinema_2.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserLog { get; set; } = null!;

    public string UserPas { get; set; } = null!;

    public string UserName { get; set; } = null!;
}
