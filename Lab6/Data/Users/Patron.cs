﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace Lab6
{
    public class Patron : Agents
    {
        // Fields
        public string Name { get; set; }
        BlockingCollection<string> behaviours { get; set; }
        private List<string> namesList = new List<string>()
        {
            "Andreas",
            "Erik",
            "Johan",
            "Magnus",
            "Olle",
            "Peter",
            "Daniel",
            "Matthias",
            "Johannes",
            "Robert",
            "Erica",
            "Matilda",
            "Sofia",
            "Rebecka",
            "Eva",
            "Linnea",
            "Anna",
            "Johanna",
            "Amanda",
            "Jennie"
        };


        public string Behaviours()
        {
            behaviours = new BlockingCollection<string>()
            {
                $"{Name} kommer in och går till baren",
                $"{Name}Väntar på servering",
                $"{Name} letar efter stol",
                $"{Name} sitter och dricker öl",
                $"{Name} har druckit upp och lämnar baren"
            };
            return behaviours.Take();
        }

        public string Names()
        {
            Random random = new Random();

            var name = namesList[random.Next(namesList.Count())];
            return name;
        }

        public Patron()
        {
            Name = Names();
        }
    }
}
