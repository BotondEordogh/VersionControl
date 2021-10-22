using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValueAtRisk
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList(); //Fontoos, ha nem ToList-eljük, akkor az SQL adatbázisból számolja a dolgokat nem a VS-ből, lokális memóriából
            dataGridView1.DataSource = Ticks;
        }
    }
}
