using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;

namespace FileDuplicate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// first use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_Loaded(object sender, RoutedEventArgs e)
        {
            txtInput.Foreground = Brushes.Gray;
            txtInput.Text = "Input your directory";
        }

        /// <summary>
        /// focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            txtInput.Foreground = Brushes.Blue;
            txtInput.Text = "";
        }

        /// <summary>
        /// event click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            // Ví dụ yêu cầu :
            // anh1.jpg
            // anh1 (1).jpg
            // anh1 (2).jpg
            // => xuất ra : anh1 (1).jpg anh1 (2).jpg

            //var path = @"C:\Users\nguyenhuuhoa\Desktop\abc\";
            var path = txtInput.Text;

            if (path.Length==0)
            {
                var button = MessageBoxButton.OK;
                var image = MessageBoxImage.Error;
                MessageBox.Show("Không thể để đường dẫn trống!", "Lỗi", button, image);
            }

            try
            {
                var info1 = new DirectoryInfo(path);
                if (!info1.Exists)
                {
                    var button = MessageBoxButton.OK;
                    var image = MessageBoxImage.Error;
                    MessageBox.Show("Đường dẫn không tồn tại!", "Lỗi", button, image);
                    txtResult.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                var button = MessageBoxButton.OK;
                var image = MessageBoxImage.Error;
                MessageBox.Show("Đường dẫn lỗi!", "Lỗi", button, image);
                txtResult.Text = "";
                return;
            }
            

            //MessageBox.Show(info.FullName);
            //if (info.Exists) MessageBox.Show("TM tồn tại"); else MessageBox.Show("TM không tồn tại");

            var res = new List<string>();
            var info = new DirectoryInfo(path);

            FileInfo[] listFile = info.GetFiles();

            //foreach (var index in listFile)
            //{
            //    MessageBox.Show(index.Name);
            //}

            var len = listFile.Length;
            for (int i=0;i<len-1;i++)
            {
                for (int j=i+1;j<len;j++)
                {
                    if (listFile[i].Extension == listFile[j].Extension && listFile[i].Name.Length != listFile[j].Name.Length)
                    {
                        var extension = listFile[i].Extension;

                        //var file1 = listFile[i].Name;
                        //var file2 = listFile[j].Name;
                        //var f1 = file1.Remove(file1.Length - 1 - extension.Length - 1, extension.Length + 1);
                        //var f2 = file2.Remove(file1.Length - 1 - extension.Length - 1, extension.Length + 1);
                        // xóa (độ dài đuôi + 1 dấu chấm) kí tự
                        // kể từ (vị trí cuối - độ dài đuôi tập tin - 1 dấu chấm)
                        // Ví dụ : abc.txt => xóa 4(=3+1) kí tự kể từ vị trí 2(=7-1-3-1)

                        var idx1 = listFile[i].Name.LastIndexOf('.'); // Tìm dấu chấm cuối cùng
                        var file1= listFile[i].Name.Remove(idx1); // xóa extension đi
                        var idx2 = listFile[j].Name.LastIndexOf('.');
                        var file2 = listFile[j].Name.Remove(idx2);

                        //MessageBox.Show(file1);
                        //MessageBox.Show(file2);

                        if (file1.Length > file2.Length)
                        {
                            //MessageBox.Show("Trường hợp 1");
                            var index1 = file1.LastIndexOf('('); // Tìm dấu ngoặc mở trong (1) hoặc (2)........
                            var index2 = file1.LastIndexOf(')'); // Tìm vị trí dấu ngoặc đóng

                            if (index1==-1 || index2==-1) // không tìm thấy, 2 file khác nhau
                            {
                                continue; // 2 file này không bị trùng nhau
                            }
                            else
                            {
                                var temp = file1.Remove(index1 - 1,index2 - index1 + 1 + 1); // - 1 khoảng cách
                                if (temp == file2) res.Add(listFile[i].Name);
                            }
                            
                        }
                        else
                        {
                            //MessageBox.Show("Trường hợp 2");
                            var index1 = file2.LastIndexOf('(');
                            var index2 = file2.LastIndexOf(')');
                            if (index1 == -1 || index2 == -1)
                            {
                                continue;
                            }
                            else
                            {
                                var temp = file2.Remove(index1 - 1, index2 - index1 + 1 + 1);
                                if (temp == file1) res.Add(listFile[j].Name);
                            }
                            
                        }
                    }
                }
            }

            var str = new StringBuilder();

            if (res.Count==0)
            {
                txtResult.Text = "Không có file trùng!";
            }
            else
            {
                str.Append("Danh sách file trùng : \n");
                foreach (var index in res)
                {
                    str.Append(index);
                    str.Append('\n');
                }
                str.Remove(str.Length - 1,1);
                txtResult.Text = str.ToString();
            }
        }

        /// <summary>
        /// event : exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Event : About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;
            MessageBox.Show("File Duplicate v1.0\nNguyễn Hữu Hòa, ĐHKHTN HCM", "Thông tin", button, icon);
        }

        private void Use_Click(object sender, RoutedEventArgs e)
        {
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;
            MessageBox.Show("1. Chọn thư mục cần tìm vào ô text\n2. Nhấn Find", "Hướng dẫn", button, icon);
        }
    }
}
