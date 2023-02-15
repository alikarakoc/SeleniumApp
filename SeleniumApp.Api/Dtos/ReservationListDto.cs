namespace SeleniumApp.Api.Dtos
{
    public class ReservationListDto
    {
        public string Agency { get; set; }
        public string Hotel { get; set; }
        public string Operator { get; set; }
        public string Voucher { get; set; }
        public string RoomType { get; set; }
        public string NoteMessage { get; set; }
        public string WarningMessage { get; set; }
    }
}
