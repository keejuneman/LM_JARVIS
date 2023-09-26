using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LM_JARVIS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        private string currentDate;
        private string time_selected;
        private string ScreenShotFileName;
        private string settingsFilePath = "setting.ini";
        private string savedFolderPath;
        private bool subMonitorExists = false;

        public MainWindow()
        {
            InitializeComponent();
            currentDate = DateTime.Now.ToString("yyMMdd");
            TEXT_화면캡쳐_날짜.Text = currentDate;

            // savedFolderPath를 설정 파일에서 읽어옵니다.
            if (System.IO.File.Exists(settingsFilePath))
            {
                savedFolderPath = System.IO.File.ReadAllText(settingsFilePath);
                TEXT_화면캡쳐_경로.Text = savedFolderPath;
            }
            else
            {
                // 설정 파일이 없는 경우, 기본 폴더 경로를 지정하고 파일을 생성
                savedFolderPath = @"C:\DefaultFolderPath"; // 기본 폴더 경로 설정
                System.IO.File.WriteAllText(settingsFilePath, savedFolderPath);
                TEXT_화면캡쳐_경로.Text = savedFolderPath;
            }
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
        private void LISTBOX_화면캡쳐_리스트박스_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LISTBOX_화면캡쳐_리스트박스.SelectedItem != null)
            {
                string selectedItem = LISTBOX_화면캡쳐_리스트박스.SelectedItem.ToString();
                time_selected = selectedItem;
            }
        }


        private void BUTTON_화면캡쳐_리스트추가_Click(object sender, RoutedEventArgs e)
        {
            LISTBOX_화면캡쳐_리스트박스.Items.Add(TEXT_화면캡쳐_리스트추가.Text);
            TEXT_화면캡쳐_리스트추가.Text = "";
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

                // savedFolderPath를 업데이트합니다.
                savedFolderPath = selectedFolderPath;
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
                savedFolderPath = System.IO.File.ReadLines(settingsFilePath).FirstOrDefault();
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


        private string GenerateUniqueFileName(string folderPath, string fileNameWithoutExtension, string fileformat)
        {
            string uniqueFileName = $"{fileNameWithoutExtension} {fileformat}";
            int counter = 1;

            while (true)
            {
                string filePath = System.IO.Path.Combine(folderPath, uniqueFileName);
                if (!File.Exists(filePath))
                {
                    return uniqueFileName;
                }

                uniqueFileName = $"{fileNameWithoutExtension} ({counter}) {fileformat}";
                counter++;
            }
        }


        private void BUTTON_화면캡쳐_캡쳐_Click(object sender, RoutedEventArgs e)
        {
            // 콤보박스에서 선택된 항목을 확인하여 캡처 대상 모니터를 결정
            Screen targetScreen = null;

            // 모든 모니터에 대한 정보 가져오기
            Screen[] screens = Screen.AllScreens;

            if (subMonitorExists && COMOBO_화면캡쳐_모니터.SelectedItem != null && COMOBO_화면캡쳐_모니터.SelectedItem.ToString() == "Sub")
            {
                // "Sub" 모니터를 캡처
                targetScreen = screens.FirstOrDefault(s => s.Primary == false);
            }
            else
            {
                // "Main" 모니터 또는 서브 모니터가 없을 때 "Main" 모니터를 캡처
                targetScreen = screens.FirstOrDefault(s => s.Primary == true);
            }

            if (targetScreen != null)
            {
                // currentDate를 이름으로 하는 디렉토리 경로 생성
                string currentDateDirectory = System.IO.Path.Combine(savedFolderPath, currentDate);

                try
                {
                    // currentDate 폴더가 없는 경우 폴더를 생성합니다.
                    if (!System.IO.Directory.Exists(currentDateDirectory))
                    {
                        System.IO.Directory.CreateDirectory(currentDateDirectory);
                    }

                    // 캡처할 모니터를 지정하고 스크린샷 캡처
                    using (Bitmap bmp = new Bitmap(targetScreen.Bounds.Width, targetScreen.Bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(targetScreen.Bounds.Location, new System.Drawing.Point(0, 0), targetScreen.Bounds.Size);
                        }

                        // 파일 이름 생성 및 중복 검사
                        ScreenShotFileName = $"{currentDate} {time_selected}";
                        string filenameformat = ".png";
                        ScreenShotFileName = GenerateUniqueFileName(currentDateDirectory, ScreenShotFileName, filenameformat);

                        // 캡처된 이미지를 파일로 저장
                        string screenshotPath = System.IO.Path.Combine(currentDateDirectory, ScreenShotFileName);
                        bmp.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png);

                        // 캡처 완료 메시지 표시
                        System.Windows.MessageBox.Show($"스크린샷이 저장되었습니다: {screenshotPath}");
                    }
                }
                catch (Exception ex)
                {
                    // 폴더 생성 또는 이미지 저장 중에 오류가 발생한 경우
                    System.Windows.MessageBox.Show($"스크린샷을 저장하는 동안 오류가 발생했습니다: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("캡처할 모니터를 찾을 수 없습니다.");
            }
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 콤보박스 선택 변경 이벤트 핸들러
            // 서브 모니터 존재 여부를 확인하고 "Sub" 옵션 활성화/비활성화
            Screen[] screens = Screen.AllScreens;
            subMonitorExists = screens.Length > 1;

            // 서브 모니터가 없을 경우 "Sub" 옵션 비활성화
            if (!subMonitorExists && COMOBO_화면캡쳐_모니터.SelectedItem != null && COMOBO_화면캡쳐_모니터.SelectedItem.ToString() == "Sub")
            {
                COMOBO_화면캡쳐_모니터.SelectedItem = "Main"; // "Main"으로 선택 변경
            }

            // "Sub" 옵션 활성화/비활성화
            COMOBO_화면캡쳐_모니터.IsEnabled = subMonitorExists;
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

        private void LISTBOX_화면캡쳐_실강_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
    

