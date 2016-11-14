
using SQLite4Unity3d;

public class Tables
{
    public class Background
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Level { get; set; }
        public string RoomName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int Dir { get; set; }
        public int Hierarchy { get; set; }
    }
}

