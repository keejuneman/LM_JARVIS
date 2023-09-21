using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace LM_JARVIS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string settingsFilePath = "setting.ini";
        public MainWindow()
        {
            InitializeComponent();
        }
        // MENU
        private void HyperlinkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.MenuItem menuItem && menuItem.Tag is string url)
            {
                try
                {
                    // 주어진 URL로 이동합니다.
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    // 예외 처리: 이동 실패 시에 대한 처리를 여기에 추가하세요.
                    System.Windows.MessageBox.Show($"링크를 여는 중 오류가 발생했습니다: {ex.Message}");
                }
            }
        }


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // 프로그램을 종료합니다.
            System.Windows.Application.Current.Shutdown();
        }

        private void BUTTON_화면캡쳐_폴더열기_Click(object sender, RoutedEventArgs e)
        {
            string folderPathToOpen = TEXT_화면캡쳐_경로.Text; // 이전 이벤트에서 저장한 폴더 경로를 가져옵니다.

            if (!string.IsNullOrEmpty(folderPathToOpen))
            {
                try
                {
                    // 선택한 폴더 경로를 파일 탐색기로 엽니다.
                    Process.Start("explorer.exe", folderPathToOpen);
                }
                catch (Exception ex)
                {
                    // 오류 처리 (예: 폴더를 찾을 수 없는 경우)
                    System.Windows.MessageBox.Show($"폴더를 열 수 없습니다. 오류 메시지: {ex.Message}");
                }
            }
        }




        //화면 캡쳐
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentDate = DateTime.Now.ToString("yyMMdd");

            if (sender is System.Windows.Controls.TextBox textBox)
            {
                textBox.Text = currentDate;
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // 현재 날짜를 YYMMDD 형식으로 가져옵니다.
            string currentDate = DateTime.Now.ToString("yyMMdd");

            // TextBox에 현재 날짜를 설정합니다.
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                textBox.Text = currentDate;
            }
        }


        private void BUTTON_화면캡쳐_폴더선택_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolderPath = ShowFolderDialog();
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                // 사용자가 선택한 폴더 경로를 TextBox에 표시
                TEXT_화면캡쳐_경로.Text = selectedFolderPath;

                // 선택한 폴더 경로를 설정 파일에 저장
                SaveFolderPathToSettings(selectedFolderPath);
            }
        }

        private void SaveFolderPathToSettings(string folderPath)
        {
            try
            {
                // 설정 파일에 폴더 경로를 저장
                System.IO.File.WriteAllText(settingsFilePath, folderPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"설정 파일에 폴더 경로를 저장하는 동안 오류가 발생했습니다: {ex.Message}");
            }
        }

        private string ShowFolderDialog()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    return folderDialog.SelectedPath;
                }
            }

            return null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(settingsFilePath))
            {
                // 설정 파일이 이미 존재하는 경우, 파일에서 폴더 경로를 읽어와서 TextBox에 표시
                string savedFolderPath = System.IO.File.ReadAllText(settingsFilePath);
                TEXT_화면캡쳐_경로.Text = savedFolderPath;
            }
            else
            {
                // 설정 파일이 없는 경우, 기본 폴더 경로를 지정하고 파일을 생성
                string defaultFolderPath = @"C:\DefaultFolderPath"; // 기본 폴더 경로 설정
                System.IO.File.WriteAllText(settingsFilePath, defaultFolderPath);
                TEXT_화면캡쳐_경로.Text = defaultFolderPath;
            }
        }

        private string selectedOption = "입실"; // 기본값: 입실
        private void LADIO_화면캡쳐_입실_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LADIO_화면캡쳐_중간_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LADIO_화면캡쳐_퇴실_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LADIO_화면캡쳐_실강_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LADIO_화면캡쳐_프로젝트_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LADIO_화면캡쳐_기타_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TEXT_화면캡쳐_기타_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        // 제출인원 관리
        private void TEXT_제출인원관리_참여인원_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TEXT_제출인원관리_전체인원_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BUTTON_제출인원관리_참여인원확인_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }




        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

    }
}
    

