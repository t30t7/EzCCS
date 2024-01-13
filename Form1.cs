using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzCCS
{
    public partial class Form1 : Form
    {

        private readonly int[] tiers = { 72466, 177466, 256756, 346756, 356756 };

        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonCCS_Click(object sender, EventArgs e)
        {
            int childCount = 1;
            double hourlyCap = Convert.ToDouble(numericHC.Value);
            if (checkChild3.Checked) { childCount = 3; }
            else if (checkChild2.Checked) { childCount = 2; }

            double discount = Convert.ToSingle(textDiscount.Text);
            int hours = Convert.ToInt16(textHours.Text);
            int[] days = new int[] { Convert.ToInt16(numericDays1.Value), Convert.ToInt16(numericDays2.Value), Convert.ToInt16(numericDays3.Value) };

            if (!int.TryParse(comboSession1.Text, out int session1)) session1 = 0;
            if (!int.TryParse(comboSession2.Text, out int session2)) session2 = 0;
            if (!int.TryParse(comboSession3.Text, out int session3)) session3 = 0;
            int[] sessions = new int[] { session1, session2, session3 };

            double[] fullRates = new double[] { Convert.ToSingle(numericFullRate1.Value), Convert.ToSingle(numericFullRate2.Value), Convert.ToSingle(numericFullRate3.Value) };

            List<Label> labelHouryRates = new List<Label> { labelHourly1, labelHourly2, labelHourly3 };
            List<Label> labelDailyRates = new List<Label> { labelDaily1, labelDaily2, labelDaily3 };
            List<Label> labelWeeklyRates = new List<Label> { labelWeekly1, labelWeekly2, labelWeekly3 };

            Quote quote = new Quote(discount, hours, days, sessions, fullRates, childCount, hourlyCap);

            // CCS calculation
            for (int c = 0; c < childCount; c++)
            {
                labelHouryRates[c].Text = quote.FullPriceHour[c].ToString("$0.00");
                if (quote.FullPriceHour[c] > hourlyCap) { labelHouryRates[c].ForeColor = System.Drawing.Color.Red; }
                else { labelHouryRates[c].ForeColor = System.Drawing.Color.Black; }
                labelDailyRates[c].Text = quote.FeeDaily[c].ToString("$0.00");
                labelWeeklyRates[c].Text = quote.FeeWeekly[c].ToString("$0.00");
                if (days[c] * sessions[c] > hours) { labelWeeklyRates[c].ForeColor = System.Drawing.Color.Red; }
                else { labelWeeklyRates[c].ForeColor = System.Drawing.Color.Black; }
            }
            labelTotalDaily.Text = quote.TotalFee[0].ToString("$0.00/day");
            labelTotalWeekly.Text = quote.TotalFee[1].ToString("$0.00/wk");
        }

        private void buttonCCSClear_Click(object sender, EventArgs e)
        {
            textIncome.Text = "";
            numericActivity.Value = 0;
            textDiscount.Text = "0.0";
            textHours.Text = "0";

            numericDays1.Value = 3; numericDays2.Value = 3; numericDays3.Value = 3;
            comboSession1.SelectedIndex = 2; comboSession2.SelectedIndex = 2; comboSession3.SelectedIndex = 2;
            numericFullRate1.Value = 0; numericFullRate2.Value = 0; numericFullRate3.Value = 0;

            labelHourly1.Text = "$0.00"; labelHourly2.Text = "$0.00"; labelHourly3.Text = "$0.00";
            labelDaily1.Text = "$0.00"; labelDaily2.Text = "$0.00"; labelDaily3.Text = "$0.00";
            labelWeekly1.Text = "$0.00"; labelWeekly2.Text = "$0.00"; labelWeekly3.Text = "$0.00";
            labelTotalDaily.Text = "$0.00/day"; labelTotalWeekly.Text = "$0.00/wk";

            checkChild2.Checked = false;
            checkChild3.Checked = false;
            checkClone.Checked = true;
        }

        private void IncomeChange()
        {
            double discount;
            try
            {
                // Discount calc
                int income = Convert.ToInt32(textIncome.Text);
                discount = 90.0 - Math.Max(Convert.ToInt32(textIncome.Text) - 80000.0, 0) / 5000.0;

                textDiscount.Text = discount.ToString("0.0");
            }
            catch
            {
                textDiscount.Text = "0.0";
            }

            UpdateActivity();
        }

        private void textIncome_TextChanged(object sender, EventArgs e)
        {
            IncomeChange();
        }

        private void numericActivity_ValueChanged(object sender, EventArgs e)
        {
            UpdateActivity();
        }

        private void UpdateActivity()
        {
            int hours;

            try
            {
                // Hours calc
                int income = Convert.ToInt32(textIncome.Text);
                int activity = Convert.ToInt16(numericActivity.Value);

                if (activity <= 8)
                {
                    if (income <= tiers[0])
                    {
                        hours = 12;
                    }
                    else { hours = 0; }
                }
                else if (activity <= 16) { hours = 18; }
                else if (activity <= 24) { hours = 36; }
                else { hours = 50; }

                textHours.Text = hours.ToString();
            }
            catch
            {
                textHours.Text = "0";
            }
        }

        private void checkChild2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkChild2.Checked)
            {
                numericDays2.Enabled = true;
                comboSession2.Enabled = true;
                numericFullRate2.Enabled = true;
                labelHourly2.Enabled = true;
                labelDaily2.Enabled = true;
                labelWeekly2.Enabled = true;
                checkChild3.Enabled = true;
            }
            else
            {
                numericDays2.Enabled = false;
                comboSession2.Enabled = false;
                numericFullRate2.Enabled = false;
                labelHourly2.Enabled = false;
                labelDaily2.Enabled = false;
                labelWeekly2.Enabled = false;
                checkChild3.Enabled = false;
            }
        }

        private void checkChild3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkChild3.Checked)
            {
                numericDays3.Enabled = true;
                comboSession3.Enabled = true;
                numericFullRate3.Enabled = true;
                labelHourly3.Enabled = true;
                labelDaily3.Enabled = true;
                labelWeekly3.Enabled = true;
            }
            else
            {
                numericDays3.Enabled = false;
                comboSession3.Enabled = false;
                numericFullRate3.Enabled = false;
                labelHourly3.Enabled = false;
                labelDaily3.Enabled = false;
                labelWeekly3.Enabled = false;
            }
        }

        private void numericDays1_ValueChanged(object sender, EventArgs e)
        {
            CCSChange(1);
        }

        private void comboSession1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CCSChange(2);
        }

        private void CCSChange(int control)
        {
            if (checkClone.Checked)
            {
                if (control == 1)
                {
                    numericDays2.Value = numericDays1.Value;
                    numericDays3.Value = numericDays1.Value;
                }
                else if (control == 2)
                {
                    comboSession2.Text = comboSession1.Text;
                    comboSession3.Text = comboSession1.Text;
                }
            }
        }

        private void checkPriceChange_CheckedChanged(object sender, EventArgs e)
        {
            IncomeChange();
        }
    }
}
