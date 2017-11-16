﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int increment = 1;
        public static Items<Chair> chairs;
        public static Items<Glass> glasses;
        public static Items<UsedGlass> usedGlasses;
        Time timer = new Time();
        private static CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationToken ct = cts.Token;

        public MainWindow()
        {
            InitializeComponent();

            // instantiate the generic Item variables
            chairs = new Items<Chair>();
            glasses = new Items<Glass>();
            usedGlasses = new Items<UsedGlass>();

            usedGlasses.item.maxNumOfUsedGlasses = glasses.item.maxNumOfGlasses;

            // Creating x amounts of items through function call.
            chairs.CreateItems(new Chair(), chairs.item.maxNumOfChairs);
            glasses.CreateItems(new Glass(), glasses.item.maxNumOfGlasses);
        }

        private void BtnOpenCloseBar_Click(object sender, RoutedEventArgs e)
        {
            timer.RunTimer(20);
            if (BtnOpenCloseBar.Content.ToString() == ("Open"))
                BtnOpenCloseBar.Content = "Close";

            else
                BtnOpenCloseBar.Content = "Open";

            Bouncer b = new Bouncer(timer);
            Bartender bartender = new Bartender();
            Waitress waitress = new Waitress(10000, 12000, 3000);

            // Bouncer/Patron Thread
            Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    b.Run(AddList);
                }
            });

            // Bartender Thread
            Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    bartender.Handling(glasses, AddList);
                }
            });

            // Waitress Thread
            Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    waitress.Handling(usedGlasses, chairs, AddList);
                }
            });

            Task.Run(() =>
            {
                UpdateLabels();
            });
        }

      
        private void AddList(string action, object sender)
        {
            action = $"{increment++} {action}";
            Dispatcher.Invoke(() =>
            {
                switch (sender)
                {
                    case Patron _:
                        GuestListBox.Items.Insert(0, action);
                        break;
                    case Bartender _:
                        BartenderListBox.Items.Insert(0, action);
                        break;
                    case Waitress _:
                        WaiterListBox.Items.Insert(0, action);
                        break;
                    case Bouncer _:
                        GuestListBox.Items.Insert(0, action);
                        break;
                }
            });
        }

        private void UpdateLabels()
        {
            while (!ct.IsCancellationRequested)
            {
                Dispatcher.Invoke(() =>
                {
                    GuestLabel1.Content = $"Number of guests: {Patron.numOfGuests}";
                    ChairLabel.Content = $"Number of Chairs: {chairs.GetNumOfItems()}";
                    GlassLabel.Content = $"Number of glasses: {glasses.GetNumOfItems()}";
                    TimerLabel.Content = timer.CurrentTime;
                });
            }
        }

        private void GuestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

    }
}

