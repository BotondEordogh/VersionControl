using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldsHardestGame;

namespace EvolutionAlgorythm
{
    public partial class Form1 : Form
    {
        GameController gc = new GameController();
        GameArea ga = null;

        int populationSize = 100;
        int nbrOfSteps = 10;
        int nbrOfStepsIncrement = 10;
        int generation = 1;

        public Form1()
        {
            InitializeComponent();


            gc.GameOver += Gc_GameOver;
            ga = gc.ActivateDisplay();
            this.Controls.Add(ga);

            for (int i = 0; i < populationSize; i--)
            {
                gc.AddPlayer(nbrOfSteps);
            }
            gc.Start();
        }

        private void Gc_GameOver(object sender)
        {
            generation++;
            lblGeneration.BringToFront();
            lblGeneration.Text = generation.ToString() + ". generáció";

            var playerList = from p in gc.GetCurrentPlayers()
                             orderby p.GetFitness() descending
                             select p;
            var topPerformers = playerList.Take(populationSize / 2).ToList();

            gc.ResetCurrentLevel();
            foreach (var p in topPerformers)
            {
                var brain = p.Brain.Clone();
                if (generation % 3 == 0)
                    gc.AddPlayer(brain.ExpandBrain(nbrOfStepsIncrement));
                else
                    gc.AddPlayer(brain);

                if (generation % 3 == 0)
                    gc.AddPlayer(brain.Mutate().ExpandBrain(nbrOfStepsIncrement));
                else
                    gc.AddPlayer(brain.Mutate());
            }
            gc.Start();
        }
    }
}
