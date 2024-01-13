using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzCCS
{
    class Quote
    {
        private readonly double[] Discount = new double[3];
        public double[] FullPriceHour = new double[3];
        public double[] Sub = new double[3];
        public double[] FeeDaily = new double[3];
        public double[] FeeWeekly = new double[3];
        public double[] TotalFee = new double[2];

        public Quote(double dc, int hrs, int[] days, int[] session, double[] rate, int count, double hc)
        {
            for (int c = 0; c < count; c++)
            {
                if (c == 0) { Discount[c] = dc; }
                else { Discount[c] = Math.Min((dc + 30), 95); }

                FullPriceHour[c] = rate[c] / session[c];
                Sub[c] = Math.Min(hc, FullPriceHour[c]) * (Discount[c] / 100) * Math.Min((days[c] * session[c]), hrs) * 0.95;
                FeeWeekly[c] = (rate[c] * days[c]) - Sub[c];
                FeeDaily[c] = FeeWeekly[c] / days[c];

                TotalFee[0] += FeeDaily[c];
                TotalFee[1] += FeeWeekly[c];
            }
        }
    }
}
