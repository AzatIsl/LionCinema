using SQLite;
using System;
using System.Collections.Generic;

using System.Text;

namespace Kursach_Ras
{
    class Collection
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
}
