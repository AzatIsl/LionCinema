using SQLite;


namespace Kursach_Ras
{
    [Table("Movie")]
    public class Movie
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public byte[] Image { get; set; }
    }
}

