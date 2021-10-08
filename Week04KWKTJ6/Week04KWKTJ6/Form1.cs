﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Week04KWKTJ6
{
    public partial class Form1 : Form
    {
        RealEstateEntities context = new RealEstateEntities();
        List <Flat> lakasok;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.Datasource = lakasok;
        }

        public void LoadData()
        {
            lakasok = context.Flats.ToList();
        }
    }
}
