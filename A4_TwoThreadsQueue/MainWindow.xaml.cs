using System;
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
using System.Windows.Threading;

namespace A4_TwoThreadsQueue {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow: Window {
		public MainWindow() {
			InitializeComponent();
			_timer.Interval = new TimeSpan(156250);
			_timer.Tick += TimerTick;
		}

		readonly DispatcherTimer _timer = new();
		readonly Queue<Point> _queue = new();

		void TimerTick(object sender, EventArgs e) {
			if(_queue.Count != 0)
				Graph.Points.Add(_queue.Dequeue());
		}

		void PlotGraph(object state) {
			const double mul = 128;
			for(double x = 0;x < 1920 / mul;x += .01) {
				if(_queue.Count > 9)
					Thread.Sleep(1);
				_queue.Enqueue(new Point(x * mul, -(23 * (x * x) - 33)));
			}
		}

		void Window_Loaded(object sender, RoutedEventArgs e) {
			Canvas.SetTop(Graph, GraphCanvas.ActualHeight / 2);
			ThreadPool.QueueUserWorkItem(PlotGraph);
			_timer.Start();
		}
	}
}
