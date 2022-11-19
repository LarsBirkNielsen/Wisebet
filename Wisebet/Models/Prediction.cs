using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wisebet.Models
{
    public class Prediction
    {
        [Key]
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM-yyyy}")]

        [DisplayName("Kick Off")]
        public DateTime KickOff { get; set; }
        public string League { get; set; }

        [DisplayName("Home Team")]
        public string HomeTeam { get; set; }

        [DisplayName("Away Team")]
        public string AwayTeam { get; set; }
        public int Tip { get; set; }
        public string Odds { get; set; }
        public double Stake { get; set; }
        public double Profit { get; set; }
        public string Status { get; set; }
    }
}
