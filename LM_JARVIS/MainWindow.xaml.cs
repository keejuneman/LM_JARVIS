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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LM_JARVIS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //스크린샷
        private string currentDate;
        private string time_selected;
        private string ScreenShotFileName;
        private string settingsFilePath = "setting.ini";
        private string savedFolderPath;
        private string radiotextbox;
        private bool subMonitorExists = false;


        //제출인원관리
        private string membersFilePath = "member.ini";
        private string categoriFilePath = "categori.ini";

        //랜덤조편성
        private int counter = 0;


        public MainWindow()
        {
            InitializeComponent();

            //스크린샷
            currentDate = DateTime.Now.ToString("yyMMdd");
            TEXT_화면캡쳐_날짜.Text = currentDate;
            radiotextbox = TEXT_화면캡쳐_라디오기타.Text;
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


            //제출인원관리
            try
            {
                if (File.Exists(membersFilePath))
                {
                    // 파일이 존재하면 파일에서 데이터를 읽어와서 텍스트 박스에 설정
                    string membersText = File.ReadAllText(membersFilePath);
                    TEXT_제출인원관리_전체인원.Text = membersText;
                    TEXT_랜덤조편성_전체인원.Text = membersText;
                }
                else
                {
                    System.IO.File.WriteAllText("member.ini", "");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"파일 불러오기 중 오류 발생: {ex.Message}");
            }
            try
            {
                if (File.Exists(categoriFilePath))
                {
                    // 파일이 존재하면 파일에서 데이터를 읽어와서 텍스트 박스에 설정
                    string membersText = File.ReadAllText(categoriFilePath);
                    TEXT_카운트_카테고리.Text = membersText;
                }
                else
                {
                    System.IO.File.WriteAllText("categoriFilePath.ini", "");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"파일 불러오기 중 오류 발생: {ex.Message}");
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





        //화면 캡쳐

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

        private void Radio_control(object sender, RoutedEventArgs e)
        {
            if (RADIO_입실.IsChecked == true)
            {
                time_selected = "입실";
            }
            else if (RADIO_중간.IsChecked == true)
            {
                time_selected = "중간";
            }
            else if (RADIO_퇴실.IsChecked == true)
            {
                time_selected = "퇴실";
            }
            else if (RADIO_실강.IsChecked == true)
            {
                time_selected = "실강";
            }
            else if (RADIO_플젝.IsChecked == true)
            {
                time_selected = "프로젝트";
                TEXT_화면캡쳐_라디오기타.IsEnabled = false;
            }
            else if (RADIO_기타.IsChecked == true)
            {
                if (TEXT_화면캡쳐_라디오기타 != null)
                {
                    time_selected = radiotextbox; // 'RADIO_기타' 체크 시 radiotextbox의 값을 사용
                    TEXT_화면캡쳐_라디오기타.IsEnabled = true; // 'RADIO_기타' 체크 시 텍스트 상자 활성화
                }
            }
            else
            {
                if (TEXT_화면캡쳐_라디오기타 != null)
                {
                    TEXT_화면캡쳐_라디오기타.IsEnabled = false; // 다른 라디오 버튼 체크 시 텍스트 상자 비활성화
                }
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
        private void BUTTON_제출인원관리_인원저장_Click(object sender, RoutedEventArgs e)
        {
            string contentToWrite = TEXT_제출인원관리_전체인원.Text; // 텍스트 박스의 내용 가져오기

            try
            {
                File.WriteAllText(membersFilePath, contentToWrite);
                System.Windows.MessageBox.Show("인원 정보가 저장되었습니다.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"파일 쓰기 중 오류 발생: {ex.Message}");
            }
        }
        private void BUTTON_제출인원관리_참여인원확인_Click(object sender, RoutedEventArgs e)
        {
            // "TEXT_제출인원관리_전체인원" 텍스트를 가져와서 개별 인원을 추출합니다.
            string allMembersText = TEXT_제출인원관리_전체인원.Text;
            string[] allMembers = allMembersText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // "TEXT_제출인원관리_참여인원" 텍스트를 가져와서 개별 인원을 추출합니다.
            string participatingMembersText = TEXT_제출인원관리_참여인원.Text;
            string[] participatingMembers = participatingMembersText.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // "TEXT_제출인원관리_결과" 텍스트 박스를 초기화합니다.
            TEXT_제출인원관리_결과.Text = "";

            // "TEXT_제출인원관리_참여인원"에 없는 인원을 찾아서 "TEXT_제출인원관리_결과"에 추가합니다.
            foreach (string member in allMembers)
            {
                if (!participatingMembers.Contains(member))
                {
                    TEXT_제출인원관리_결과.Text += "@" + member + " ";
                }
            }

            // 클립보드에 "TEXT_제출인원관리_결과"의 내용을 복사합니다.
            System.Windows.Clipboard.SetText(TEXT_제출인원관리_결과.Text);
            System.Windows.MessageBox.Show("결과가 복사 되었습니다.");
        }




        private void BUTTON_카운트_카테고리저장_Click(object sender, RoutedEventArgs e)
        {
            string contentToWrite2 = TEXT_카운트_카테고리.Text; // 텍스트 박스의 내용 가져오기

            try
            {
                File.WriteAllText(categoriFilePath, contentToWrite2);
                System.Windows.MessageBox.Show("인원 정보가 저장되었습니다.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"파일 쓰기 중 오류 발생: {ex.Message}");
            }
        }

        private void BUTTON_카운트_카운팅_Click(object sender, RoutedEventArgs e)
        {
            string itemsText = TEXT_카운트_항목.Text;
            string categoryText = TEXT_카운트_카테고리.Text;
            Dictionary<string, int> categoryCounts = new Dictionary<string, int>();

            // "TEXT_카운트_항목" 텍스트를 쉼표 및 줄바꿈으로 나누어 항목 목록을 생성
            string[] items = itemsText.Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(item => item.Trim())
                                      .ToArray();

            // "TEXT_카운트_카테고리" 텍스트를 줄바꿈 문자로 나누어 카테고리 목록을 생성
            string[] categories = categoryText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(category => category.Trim())
                                              .ToArray();

            // 각 칸의 초기값은 0으로 설정.
            foreach (string category in categories)
            {
                categoryCounts[category] = 0;
            }

            // "기타" 분류도 초기화.
            categoryCounts["기타"] = 0;

            foreach (string item in items)
            {
                bool matched = false;

                foreach (string category in categories)
                {
                    if (item.Equals(category))
                    {
                        categoryCounts[category]++;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    // 일치하는 항목이 없으면 "기타"에 추가.
                    categoryCounts["기타"]++;
                }
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (var pair in categoryCounts)
            {
                resultBuilder.AppendLine($"{pair.Key} : {pair.Value}");
            }

            TEXT_카운트_결과.Text = resultBuilder.ToString();
        }

        private void BUTTON_랜덤조편성_인원저장_Click(object sender, RoutedEventArgs e)
        {
            string contentToWrite = TEXT_랜덤조편성_전체인원.Text; // 텍스트 박스의 내용 가져오기

            try
            {
                File.WriteAllText(membersFilePath, contentToWrite);
                System.Windows.MessageBox.Show("인원 정보가 저장되었습니다.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"파일 쓰기 중 오류 발생: {ex.Message}");
            }

        }




        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            counterTextBlock.Text = counter.ToString();
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (counter > 0)
            {
                counter--;
                counterTextBlock.Text = counter.ToString();
            }
        }


        private void BUTTON_랜덤조편성_조편성_Click(object sender, RoutedEventArgs e)
        {
            string[] members = TEXT_랜덤조편성_전체인원.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            members = members.Select(member => member.Trim()).Where(member => !string.IsNullOrEmpty(member)).ToArray();

            int selectedValue = int.Parse(counterTextBlock.Text);

            Random rng = new Random();
            List<List<string>> teams = new List<List<string>>();

            if (RADIO_랜덤조편성_팀갯수.IsChecked == true)
            {
                members = members.OrderBy(x => rng.Next()).ToArray();

                // 선택된 값만큼의 팀 리스트 생성
                for (int i = 0; i < selectedValue; i++)
                    teams.Add(new List<string>());

                // 멤버들을 순서대로 각 팀에 배분
                for (int i = 0; i < members.Length; i++)
                    teams[i % selectedValue].Add(members[i]);

            }
            else
            {
                members = members.OrderBy(x => rng.Next()).ToArray();

                for (int i = 0; i < members.Length; i++)
                {
                    if (i % selectedValue == 0)
                        teams.Add(new List<string>());

                    teams.Last().Add(members[i]);
                }
            }

            StringBuilder resultBuilder = new StringBuilder();
            for (int teamNo = 0; teamNo < teams.Count; teamNo++)
            {
                resultBuilder.AppendLine($"{teamNo + 1}조 : {string.Join(", ", teams[teamNo])}");
            }

            TEXT_랜덤조편성_결과.Text = resultBuilder.ToString();
        }



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