using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wisebet.Authorization;
using Wisebet.Data;
using Wisebet.Models;

namespace Wisebet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            GetCountValues();
            GetRecentPredictions();
            LeagueStats();
            GetPendingCount();
            GetTotalStats();
        }

        public bool isAnyPendingAttention;
        private void GetPendingCount()
        {
            int pendingCount = _context.Prediction.Where(x => x.KickOff < DateTime.Now).Count();
            if(pendingCount > 0)
            {
                isAnyPendingAttention = true;
            }    
        }

        public int  TotalPredictionsPlayed, WinsCount, LostCount, PendingCount;
        private void GetCountValues()
        {
            //Values For Stats Section
            WinsCount = _context.Prediction.Count(prediction => prediction.Status == "Won");
            LostCount = _context.Prediction.Count(prediction => prediction.Status == "Lost");
            PendingCount = _context.Prediction.Count(prediction => prediction.Status == "Pending");
            TotalPredictionsPlayed = (WinsCount + LostCount);       
        }

        //Performance table
        public List<Prediction> RecentPredictions = new List<Prediction>();
        private void GetRecentPredictions()
        {
            RecentPredictions = _context.Prediction.Where(prediction => prediction.Status != "Pending").ToList();
            RecentPredictions = RecentPredictions.OrderByDescending(prediction => prediction.Id).Take(5).ToList();
        }



        public int PremierLeagueTotalPredictions, PremierLeagueWins, PremierLeagueLost;
        public double PremierLeagueHitRate, PremierLeagueProfit, PremierLeagueProfitLost, PremierLeagueTotal, PremierLeagueTotalStake;

        public int SerieATotalPredictions, SerieAWins, SerieALost;
        public double SerieAHitRate, SerieAProfit, SerieAProfitLost, SerieATotal, SerieATotalStake;

        public int LaLigaTotalPredictions, LaLigaWins, LaLigaLost;
        public double LaLigaHitRate, LaLigaProfit, LaLigaProfitLost, LaLigaTotal, LaLigaTotalStake;

        public int Ligue1TotalPredictions, Ligue1Wins, Ligue1Lost;
        public double Ligue1HitRate, Ligue1Profit, Ligue1ProfitLost, Ligue1Total, Ligue1TotalStake;

        public int PrimeiraTotalPredictions, PrimeiraWins, PrimeiraLost;
        public double PrimeiraHitRate, PrimeiraProfit, PrimeiraProfitLost, PrimeiraTotal, PrimeiraTotalStake;

        private void LeagueStats()
        {
            List<Prediction> predictions = _context.Prediction.ToList();


            foreach (Prediction prediction in predictions)
            {
                if (prediction.League.Equals("Premier League"))
                {
                    PremierLeagueTotalPredictions++;

                    if (prediction.Status == "Won")
                    {
                        PremierLeagueWins++;
                        PremierLeagueProfit = Math.Round(PremierLeagueProfit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake);
                    }
                    else if (prediction.League.Equals("Premier League") && prediction.Status == "Lost")
                    {
                        PremierLeagueLost++;
                        PremierLeagueProfit = Math.Round(PremierLeagueProfit + prediction.Stake * -1);
                    }
                    else
                    {
                        PremierLeagueProfit = Math.Round(PremierLeagueProfit + 0);
                    }
                    PremierLeagueHitRate = Math.Round(((double)PremierLeagueWins / (double)PremierLeagueTotalPredictions) * 100);
                }
                if (prediction.League.Equals("Serie A"))
                {
                    SerieATotalPredictions++;

                    if (prediction.Status == "Won")
                    {
                        SerieAWins++;
                        SerieAProfit = Math.Round(SerieAProfit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake);
                    }
                    else if (prediction.Status == "Lost")
                    {
                        SerieALost++;
                        SerieAProfit = Math.Round(SerieAProfit + prediction.Stake * -1);
                    }
                    else
                    {
                        SerieAProfit = Math.Round(SerieAProfit + 0);
                    }
                    SerieAHitRate = Math.Round(((double)SerieAWins / (double)SerieATotalPredictions) * 100);
                }
                if (prediction.League.Equals("Ligue 1"))
                {
                    Ligue1TotalPredictions++;

                    if (prediction.Status == "Won")
                    {
                        Ligue1Wins++;
                        Ligue1Profit = Math.Round(Ligue1Profit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake);
                    }
                    else if (prediction.Status == "Lost")
                    {
                        Ligue1Lost++;
                        Ligue1Profit = Math.Round(Ligue1Profit + prediction.Stake * -1);
                    }
                    else
                    {
                        Ligue1Profit = Math.Round(Ligue1Profit + 0);
                    }
                    Ligue1HitRate = Math.Round(((double)Ligue1Wins / (double)Ligue1TotalPredictions) * 100);
                    Console.WriteLine(Ligue1Profit);
                }
                if (prediction.League.Equals("La Liga"))
                {
                    LaLigaTotalPredictions++;

                    if (prediction.Status == "Won")
                    {
                        LaLigaWins++;
                        LaLigaProfit = Math.Round(LaLigaProfit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake);
                    }
                    else if (prediction.Status == "Lost")
                    {
                        LaLigaLost++;
                        LaLigaProfit = Math.Round(LaLigaProfit + prediction.Stake * -1);
                    }
                    else
                    {
                        LaLigaProfit = Math.Round(LaLigaProfit + 0);
                    }
                    LaLigaHitRate = Math.Round(((double)LaLigaWins / (double)LaLigaTotalPredictions) * 100);
                }
                if (prediction.League.Equals("Primeira Liga"))
                {
                    PremierLeagueTotalPredictions++;
                    if (prediction.Status == "Won")
                    {
                        PremierLeagueWins++;
                        PrimeiraProfit = Math.Round(PrimeiraProfit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake);
                    }
                    else if (prediction.Status == "Lost")
                    {
                        PrimeiraLost++;
                        PrimeiraProfit = Math.Round(PrimeiraProfit + prediction.Stake * -1);
                    }
                    else
                    {
                        PrimeiraProfit = Math.Round(PrimeiraProfit + 0);
                    }
                    PrimeiraHitRate = Math.Round(((double)PrimeiraWins / (double)PrimeiraTotalPredictions) * 100);
                }
            }
        }

        public double TotalProfit, TotalHitrate, TotalRoi;
        private void GetTotalStats()
        {
            double totalWager = 0;
            List<Prediction> predictions = _context.Prediction.ToList();

            TotalHitrate = Math.Round(((double)WinsCount / (double)TotalPredictionsPlayed) * 100);

            foreach(var prediction in predictions)
            {
                if(prediction.Status == "Won")
                {
                    TotalProfit = TotalProfit + (prediction.Stake * Double.Parse(prediction.Odds)) - prediction.Stake;
                    totalWager = totalWager + prediction.Stake;
                }
                else if(prediction.Status == "Lost")
                {
                    TotalProfit = TotalProfit + prediction.Stake * -1;
                    totalWager = totalWager + prediction.Stake;
                }
            }
            TotalRoi = Math.Round((TotalProfit / (TotalPredictionsPlayed * totalWager) * 100),2)+1;
        }
    }
}