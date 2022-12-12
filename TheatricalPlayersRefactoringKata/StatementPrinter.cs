using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        private int totalCost;
        private int rewardPoints;
        private int audienceCount;
        private int costOfPlay;
        private string statement;
        private Play play; 
        private CultureInfo cultureInfo;


        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {

            Init(invoice);

            foreach(var performance in invoice.Performances) 
            {
                ResetFields(performance, plays);

                CalculateCosts();

                AddRewardPoints();

                PrintOrderStatement();
            }

            FormatStatement();

            return statement;
        }
        
        private void Init(Invoice invoice) {
            totalCost = 0;
            rewardPoints = 0;
            statement = string.Format("Statement for {0}\n", invoice.Customer);
            cultureInfo = new CultureInfo("en-US");
        }

        private void AddRewardPoints() {
                rewardPoints += Math.Max(audienceCount - 30, 0);
                if ("comedy" == play.Type) rewardPoints += (int)Math.Floor((decimal)audienceCount / 5);
        }

        private void ResetFields(Performance performance, Dictionary<string, Play> plays) {
                audienceCount = performance.Audience;
                play = plays[performance.PlayID];
                costOfPlay = 0;
        }

        private void CalculateCosts() {
        switch (play.Type) 
                {
                    case "tragedy":
                        costOfPlay = 40000;
                        if (audienceCount > 30) {
                            costOfPlay += 1000 * (audienceCount - 30);
                        }
                        break;
                    case "comedy":
                        costOfPlay = 30000;
                        if (audienceCount > 20) {
                            costOfPlay += 10000 + 500 * (audienceCount - 20);
                        }
                            costOfPlay += 300 * audienceCount;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
        }

        private void PrintOrderStatement() {
                statement += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(costOfPlay / 100), audienceCount);
                totalCost += costOfPlay;

        }

        private void FormatStatement() {
            statement += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalCost / 100));
            statement += String.Format("You earned {0} credits\n", rewardPoints);
        }
    }
}
