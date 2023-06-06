using System;
using System.Collections.Generic;

namespace LionCinema_2.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string MovieName { get; set; } = null!;

    public byte[] MovieImg { get; set; } = null!;

    public string MoviePath { get; set; } = null!;
}
