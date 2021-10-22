using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValueAtRisk.Entities;

namespace ValueAtRisk
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        List<decimal> nyeresegekRendezve;
        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList(); //Fontoos, ha nem ToList-eljük, akkor az SQL adatbázisból számolja a dolgokat nem a VS-ből, lokális memóriából
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();

            List<decimal> Nyereségek = new List<decimal>();

            int intervalum = 30;

            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = dateTimePicker1.Value;

            TimeSpan z = záróDátum - kezdőDátum;

            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            nyeresegekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyeresegekRendezve[nyeresegekRendezve.Count() / 5].ToString());
        }

        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }
        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                               && date <= x.TradingDay
                            orderby x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;

            if (sfd.ShowDialog() != DialogResult.OK)           
                return;

            using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
            {
                sw.WriteLine("Időszak;Nereségek");
                for (int i = 0; i < nyeresegekRendezve.Count(); i++)
                {
                    sw.WriteLine(string.Format(
                        "{0};{1}",
                        Math.Round((double)i / (double)nyeresegekRendezve.Count(),2),
                        nyeresegekRendezve[i]
                        ));
                }
            }
        }
    }
}
