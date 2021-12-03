using MicroSim.Entities;
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

namespace MicroSim
{
    public partial class Form1 : Form
    {
        Random rng = new Random(1234);

        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        
        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthPopulation(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathPopulation(@"C:\Temp\halál.csv");
        }
        public void StartSim (int endYear, string csvPath)
        {
            for (int year = 0; year <= endYear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }
                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                richTextBox1.Text += string.Format(
                    "Szimulációs év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales);

            }
        }

        public List<Person> GetPopulation(string csvPath)
        {
            List<Person> population = new List<Person>();
            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new Person();
                    p.BirthYear = int.Parse(line[0]);
                    p.Gender = (Gender)Enum.Parse(typeof(Gender), line[1]);
                    p.NbrOfChildren = int.Parse(line[2]);
                    population.Add(p);
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthPopulation(string csvPath)
        {
            List<BirthProbability> Bprob = new List<BirthProbability>();
            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new BirthProbability();
                    p.Age = int.Parse(line[0]);
                    p.NbrOfChildren = int.Parse(line[2]);
                    p.P = double.Parse(line[2].Replace(',', '.'));
                    Bprob.Add(p);
                }
            }

            return Bprob;
        }
        public List<DeathProbability> GetDeathPopulation(string csvPath)
        {
            List<DeathProbability> Dprob = new List<DeathProbability>();
            using (var sr = new StreamReader(csvPath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    var p = new DeathProbability();
                    p.Gender = (Gender)Enum.Parse(typeof(Gender), line[0]);
                    p.Age = int.Parse(line[1]);
                    p.P = double.Parse(line[2].Replace(',', '.'));
                    Dprob.Add(p);
                }
            }

            return Dprob;
        }
        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartSim((int)numericUpDown1.Value, textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.FileName = textBox1.Text;

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            textBox1.Text = ofd.FileName;
        }
    }
}
