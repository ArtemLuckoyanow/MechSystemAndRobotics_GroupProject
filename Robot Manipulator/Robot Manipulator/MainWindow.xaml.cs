﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robot_Manipulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Manipulator manipulator;

        public MainWindow()
        {
            InitializeComponent();
            manipulator = new Manipulator();
            ReRenderCanvas(ref canvasMain);
            manipulator.PropertyChanged += Manipulator_PropertyChanged;
            canvasMain.MouseMove += CanvasMain_MouseMove;

        }

        private void CanvasMain_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {

            }
        }

        private void CanvasMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MessageBox.Show("wefe");
        }

        private void Manipulator_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ReRenderCanvas(ref canvasMain);
        }

        static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;

        Task renderTask;
        public void RenderTaskFunction()
        {
            while(true)
            {
                if (token.IsCancellationRequested)
                {
                    MessageBox.Show("RenderOver");
                }
                ReRenderCanvas(ref canvasMain);
                Task.Delay(30);
            }

        }
        



        //Если не будет привязки придется дергать её1
        public void ReRenderCanvas(ref Canvas canvas)
        {
            canvasMain.Children.Clear();

            for (int i = 0; i < manipulator.links.Count; i++)
                canvasMain.Children.Add(manipulator.links[i]);
        }

        private void CanvasMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //Link selectedBar = (Link)e.OriginalSource; //СРавнивая с этим можно будет узнаеть есть ли пересечение.

            if(manipulator.SelectedItem != null)
            {
                Point currentPosition = Mouse.GetPosition(canvasMain);
                manipulator.ChangeSelectedLinkViaNewEndPoint(currentPosition);
            }
            




        }

        private void CanvasMain_MouseRightButtonDown_SelectLink(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Link selectedBar = (Link)e.OriginalSource;

                //manipulator.ChangeLink(ref selectedBar, testAngle, testLength);

                manipulator.SelectedItem = selectedBar;           
            }
            catch(Exception)
            {

            }
            finally
            {
                //MessageBox.Show(e.OriginalSource.ToString());
            }
        }

        const double ScaleRate = 1.1;
        private void CanvasMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            Canvas c = sender as Canvas;
            ScaleTransform st = new ScaleTransform();
            c.RenderTransform = st;
            if (e.Delta > 0)
            {
                st.ScaleX *= ScaleRate;
                st.ScaleY *= ScaleRate;
            }
            else
            {
                st.ScaleX /= ScaleRate;
                st.ScaleY /= ScaleRate;
            }
        }

        private void buttonAddLink_Click(object sender, RoutedEventArgs e)
        {
            manipulator.AddLink();
            //ReRenderCanvas(ref canvasMain);
        }

        private void buttonDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            manipulator.DeleteLink();
            //ReRenderCanvas(ref canvasMain);
        }

        private void canvasMain_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            cancelTokenSource.Cancel();
        }
    }
}
