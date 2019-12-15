using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DlhSoft.Windows.Controls;
using System.Windows.Threading;

namespace DataTreeGridSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // You may uncomment the next lines of code to test the component performance:
            // for (int i = 3; i <= 16384; i++)
            // {
            //    DataTreeGrid.Items.Add(
            //        new DataTreeGridItem
            //        {
            //            Content = "Node " + i,
            //            Indentation = i % 5 == 0 ? 0 : 1,
            //            IsExpanded = i % 10 == 0 ? false : true
            //        });
            // }
        }

        // Control area commands.
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            DataTreeGridItem item = new DataTreeGridItem { Content = "New Node" };
            DataTreeGrid.Items.Add(item);
            Dispatcher.BeginInvoke((Action)delegate
            {
                DataTreeGrid.SelectedItem = item;
            }, 
            DispatcherPriority.Background);
        }
        private void InsertNewButton_Click(object sender, RoutedEventArgs e)
        {
            DataTreeGridItem selectedItem = DataTreeGrid.SelectedItem as DataTreeGridItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Cannot insert a new item before selection as the selection is empty; you can either add a new item to the end of the list instead, or select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            DataTreeGridItem item = new DataTreeGridItem { Content = "New Node", Indentation = selectedItem.Indentation };
            DataTreeGrid.Items.Insert(DataTreeGrid.SelectedIndex, item);
            Dispatcher.BeginInvoke((Action)delegate
            {
                DataTreeGrid.SelectedItem = item;
            },
            DispatcherPriority.Background);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<DataTreeGridItem> items = new List<DataTreeGridItem>();
            foreach (object item in DataTreeGrid.GetSelectedItems())
                items.Add(item as DataTreeGridItem);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot delete the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            DataTreeGrid.BeginInit();
            foreach (DataTreeGridItem item in items)
            {
                if (item == null)
                    continue;
                if (item.HasChildren)
                {
                    MessageBox.Show(string.Format("Cannot delete {0} because it has child items; remove its child items first.", item), "Information", MessageBoxButton.OK);
                    continue;
                }
                DataTreeGrid.Items.Remove(item);
            }
            DataTreeGrid.EndInit();
        }
        private void IncreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        {
            List<DataTreeGridItem> items = new List<DataTreeGridItem>();
            foreach (object item in DataTreeGrid.GetSelectedItems())
                items.Add(item as DataTreeGridItem);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot increase indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            DataTreeGrid.BeginInit();
            foreach (DataTreeGridItem item in items)
            {
                if (item == null)
                    continue;
                int index = DataTreeGrid.IndexOf(item);
                if (index > 0)
                {
                    DataTreeGridItem previousItem = DataTreeGrid[index - 1];
                    if (item.Indentation <= previousItem.Indentation)
                        item.Indentation++;
                    else
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the previous item is its parent item; increase the indentation of its parent item first.", item), "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot increase indentation for {0} because it is the first item; insert an item before this one first.", item), "Information", MessageBoxButton.OK);
                }
            }
            DataTreeGrid.EndInit();
        }
        private void DecreaseIndentationButton_Click(object sender, RoutedEventArgs e)
        {
            List<DataTreeGridItem> items = new List<DataTreeGridItem>();
            foreach (object item in DataTreeGrid.GetSelectedItems())
                items.Add(item as DataTreeGridItem);
            if (items.Count <= 0)
            {
                MessageBox.Show("Cannot decrease indentation for the selected item(s) as the selection is empty; select an item first.", "Information", MessageBoxButton.OK);
                return;
            }
            items.Reverse();
            DataTreeGrid.BeginInit();
            foreach (DataTreeGridItem item in items)
            {
                if (item == null)
                    continue;
                if (item.Indentation > 0)
                {
                    int index = DataTreeGrid.IndexOf(item);
                    DataTreeGridItem nextItem = index < DataTreeGrid.Items.Count - 1 ? DataTreeGrid[index + 1] : null;
                    if (nextItem == null || item.Indentation >= nextItem.Indentation)
                        item.Indentation--;
                    else
                        MessageBox.Show(string.Format("Cannot increase indentation for {0} because the next item is one of its child items; decrease the indentation of its child items first.", item), "Information", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(string.Format("Cannot decrease indentation for {0} because it is on the first indentation level, being a root item.", item), "Information", MessageBoxButton.OK);
                }
            }
            DataTreeGrid.EndInit();
        }
    }
}
