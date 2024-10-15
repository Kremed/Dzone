namespace Dzone.Mobile.Dtos
{
    public class UserTbl
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string token { get; set; }
        public DateTime expairyDate { get; set; }
        public DateTime createdDate { get; set; }
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public string phone { get; set; } = "";
        public string globalID { get; set; } = "";
        public string role { get; set; } = "";
    }
}
