using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace LinkedGame
{
    public partial class GameForm : Form
    {
        //static
        private static int RowNum = 6;
        private static int ColumnNum = 8;
        private static int MarginX = 10;
        private static int MarginY = 10;
        private static Size PicSize = new Size(48, 48);
        private static int GameTime = 60;
        private static int GameTimeLevel1 = 40;
        private static int GameTimeLevel2 = 60;
        private static int GameTimeLevel3 = 100;
        private static int GameTimeLevel4 = 120;
        //will not change when initialize game.
        private Graphics gp;
        private Pen blackPen;
        private int m_DrawOffSetX = (Convert.ToInt32((PicSize.Width + 6 + MarginX) / 2));
        private int m_DrawOffSetY = (Convert.ToInt32((PicSize.Height + 6 + MarginY) / 2));
        //will change when initialize game.
        private int m_SelectedCount = 0;
        private int m_CurrentLevel = 2; //normal
        private Random m_Random;
        private Point m_StartPoint;
        private Point m_EndPoint;
        private AnimationItem[,] m_GameMatrix;
        private Point[,] m_ItemLocationMatrixInPixel;
        private int[,] m_ItemVisiabilityMatrixAroundTrue;
        private string[] imgList;

        //Menu
        private List<ClickableLabel> m_MenuItems = new List<ClickableLabel>();
        public GameForm()
        {
            m_Random = new Random();
            InitializeComponent();
            this.label4.ForeColor = Color.Green;
            InitializeThemes();
            imgList = Helpers.GetImageFiles("batman");
            gp = this.panel1.CreateGraphics();
            blackPen = new Pen(Color.Black, 2f);
            this.panel1.Visible = false;
            this.panel3.Visible = false;
            this.panel2.Visible = true;

            //Level items
            m_MenuItems.Add(clickableLabel2);
            m_MenuItems.Add(clickableLabel3);
            m_MenuItems.Add(clickableLabel4);
            m_MenuItems.Add(clickableLabel5);
        }

        private void InitializeThemes()
        {
            string[] themes = Helpers.GetImageSubFolders(System.Environment.CurrentDirectory + @"\images\");
            foreach (string theme in themes)
            {
                this.comboBox1.Items.Add(theme);
            }
            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void InitializeGame()
        {
            //easy
            if (m_CurrentLevel == 1)
            {
                GameTime = GameTimeLevel1;
            }
            //normal
            else if (m_CurrentLevel == 2)
            {
                GameTime = GameTimeLevel2;
            }
            //hard
            else if (m_CurrentLevel == 3)
            {
                GameTime = GameTimeLevel3;
            }
            //super
            else if (m_CurrentLevel == 4)
            {
                GameTime = GameTimeLevel4;
            }
            if (this.comboBox1.SelectedItem != null)
            {
                imgList = Helpers.GetImageFiles(this.comboBox1.SelectedItem.ToString());
            }
            //1.Generate all items then populate to normal location
            this.panel1.Controls.Clear();
            this.timer1.Interval = 10;
            this.timer1.Start();
            this.timer3.Interval = 1000;
            this.timer3.Start();

            m_StartPoint = new Point();
            m_EndPoint = new Point();
            m_SelectedCount = 0;
            m_ItemLocationMatrixInPixel = GenerateLocationArray();
            m_ItemVisiabilityMatrixAroundTrue = new int[RowNum + 2, ColumnNum + 2];
            m_GameMatrix = new AnimationItem[RowNum, ColumnNum];

            InitializeItems();
            ResortItems();
            ReDisplay();
            ShowMatrix();

            this.panel1.Width = m_ItemLocationMatrixInPixel[RowNum + 1, ColumnNum + 1].X + PicSize.Width + MarginX;
            this.panel1.Height = m_ItemLocationMatrixInPixel[RowNum + 1, ColumnNum + 1].Y + PicSize.Height + MarginY;
            this.Width = this.panel1.Width + 30;
            this.Height = this.panel1.Height + 80;
            //this.label4.Location = new Point(this.clickableLabel1.Location.X + this.clickableLabel1.Width + 3, this.clickableLabel1.Location.Y -10);
            //this.label4.Location = new Point();
            this.label4.ForeColor = Color.Green;
            this.label4.Visible = true;

            this.CenterToScreen();
        }

        private void InitializeItems()
        {
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    int tempIndex = (columnIndex + 1) % ColumnNum;
                    if (tempIndex < imgList.Count<string>())
                    {
                        m_GameMatrix[rowIndex, columnIndex] = CreateAnimationItem(imgList[tempIndex], new Size(PicSize.Width + 6, PicSize.Height + 6));
                    }
                    else
                    {
                        m_GameMatrix[rowIndex, columnIndex] = CreateAnimationItem("", new Size(PicSize.Width + 6, PicSize.Height + 6));
                    }
                }
            }
        }

        private Item CreateAnItem(string ImgPath, Size size)
        {
            Item item1;
            item1 = new LinkedGame.Item(size);
            item1.BackColor = System.Drawing.Color.White;
            item1.TabIndex = 0;
            item1.Size = new Size(PicSize.Width + 6, PicSize.Height + 6);
            item1.Visible = true;
            item1.SetPicture(ImgPath);
            item1.SelectedEvent += new LinkedGame.SelectHandler(this.item_SelectedEvent);
            this.panel1.Controls.Add(item1);
            return item1;
        }

        private AnimationItem CreateAnimationItem(string ImgPath, Size size)
        {
            AnimationItem item1;
            item1 = new LinkedGame.AnimationItem(size);
            item1.BackColor = System.Drawing.Color.White;
            item1.TabIndex = 0;
            item1.Size = new Size(PicSize.Width + 6, PicSize.Height + 6);
            item1.Visible = true;
            item1.SetPicture(ImgPath);
            item1.SelectedEvent += new LinkedGame.SelectHandler(this.item_SelectedEvent);
            this.panel1.Controls.Add(item1);
            return item1;
        }

        private void ReDisplay()
        {
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    if (m_GameMatrix[rowIndex, columnIndex] != null)
                    {
                        m_GameMatrix[rowIndex, columnIndex].Location = m_ItemLocationMatrixInPixel[rowIndex + 1, columnIndex + 1];
                        m_GameMatrix[rowIndex, columnIndex].RefreshOrignalLocation();
                    }
                }
            }
        }

        private void ResortItems()
        {
            if (m_GameMatrix != null)
            {
                Helpers.RadmonSort(m_GameMatrix);
            }
        }

        private Point[,] GenerateLocationArray()
        {
            Point[,] locArray = new Point[RowNum + 2, ColumnNum + 2];
            Point currentLocation = new Point(MarginX, MarginY);
            for (int rowIndex = 0; rowIndex < locArray.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < locArray.GetLength(1); columnIndex++)
                {
                    locArray[rowIndex, columnIndex] = currentLocation;
                    currentLocation.X += (PicSize.Width + MarginX);
                }
                currentLocation.Y += (PicSize.Height + MarginY);
                currentLocation.X = MarginX;
            }
            return locArray;
        }

        private void ClearSelection(int avoidRowIndex = -1, int avoidColumnIndex = -1)
        {
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    if (m_GameMatrix[rowIndex, columnIndex] != null)
                    {
                        if (avoidRowIndex == rowIndex && avoidColumnIndex == columnIndex)
                        {
                            m_StartPoint = new Point(rowIndex + 1, columnIndex + 1);
                            m_SelectedCount = 1;
                        }
                        else
                        {
                            m_GameMatrix[rowIndex, columnIndex].ClearSelection();
                        }
                    }
                }
            }
        }

        private int SelectedItemNum()
        {
            int selectNum = 0;
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    if (m_GameMatrix[rowIndex, columnIndex] != null)
                    {
                        if (m_GameMatrix[rowIndex, columnIndex].IsSelectedNow)
                        {
                            selectNum++;
                        }
                    }
                }
            }
            return selectNum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResortItems();
            ReDisplay();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label3.Text = this.panel1.PointToClient(Cursor.Position).X.ToString() + " /" + this.panel1.PointToClient(Cursor.Position).Y.ToString();
        }

        private void item_SelectedEvent(object obj, EventArgs args)
        {
            //Generate m_ItemVisiabilityMatrixAroundZero
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    if (m_GameMatrix[rowIndex, columnIndex].Visible == true)
                    {
                        m_ItemVisiabilityMatrixAroundTrue[rowIndex + 1, columnIndex + 1] = 1;
                    }
                    else
                    {
                        m_ItemVisiabilityMatrixAroundTrue[rowIndex + 1, columnIndex + 1] = 0;
                    }
                }
            }
            ShowMatrix();

            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < m_GameMatrix.GetLength(1); columnIndex++)
                {
                    //Record start and end points.
                    if (m_GameMatrix[rowIndex, columnIndex].IsSelectedNow)
                    {
                        if (rowIndex == (m_StartPoint.X - 1) && columnIndex == (m_StartPoint.Y - 1))
                        {
                            continue;
                        }
                        if (m_SelectedCount == 0)
                        {
                            m_StartPoint = new Point(rowIndex + 1, columnIndex + 1);
                            m_SelectedCount++;
                        }
                        else if (m_SelectedCount == 1)
                        {
                            m_EndPoint = new Point(rowIndex + 1, columnIndex + 1);
                            m_SelectedCount++;
                        }
                    }
                }
            }

            //get start and end point here...

            if (m_SelectedCount == 2)
            {
                Point keyPoint1 = new Point();
                Point keyPoint2 = new Point();
                AnimationItem startItem = m_GameMatrix[m_StartPoint.X - 1, m_StartPoint.Y - 1];
                AnimationItem endItem = m_GameMatrix[m_EndPoint.X - 1, m_EndPoint.Y - 1];

                if (Helpers.JudgeInLine(m_StartPoint, m_EndPoint, m_ItemVisiabilityMatrixAroundTrue)
                    && startItem.m_Img.Equals(endItem.m_Img))
                {
                    m_GameMatrix[m_StartPoint.X - 1, m_StartPoint.Y - 1].Visible = false;
                    m_GameMatrix[m_EndPoint.X - 1, m_EndPoint.Y - 1].Visible = false;
                    m_ItemVisiabilityMatrixAroundTrue[m_StartPoint.X, m_StartPoint.Y] = 0;
                    m_ItemVisiabilityMatrixAroundTrue[m_EndPoint.X, m_EndPoint.Y] = 0;
                    ClearSelection();
                    ShowMatrix();
                    Point p1 = m_ItemLocationMatrixInPixel[m_StartPoint.X, m_StartPoint.Y];
                    Point p2 = m_ItemLocationMatrixInPixel[m_EndPoint.X, m_EndPoint.Y];
                    drawLine(p1, p2);

                    m_SelectedCount = 0;
                    m_StartPoint = new Point();
                    m_EndPoint = new Point();
                }
                else if (Helpers.JudgeInRightAngle(m_StartPoint, m_EndPoint, m_ItemVisiabilityMatrixAroundTrue, out keyPoint1)
                    && startItem.m_Img.Equals(endItem.m_Img))
                {
                    m_GameMatrix[m_StartPoint.X - 1, m_StartPoint.Y - 1].Visible = false;
                    m_GameMatrix[m_EndPoint.X - 1, m_EndPoint.Y - 1].Visible = false;
                    m_ItemVisiabilityMatrixAroundTrue[m_StartPoint.X, m_StartPoint.Y] = 0;
                    m_ItemVisiabilityMatrixAroundTrue[m_EndPoint.X, m_EndPoint.Y] = 0;
                    string x1 = keyPoint1.X.ToString();
                    string y1 = keyPoint1.Y.ToString();
                    ClearSelection();
                    ShowMatrix();
                    Point p1 = m_ItemLocationMatrixInPixel[m_StartPoint.X, m_StartPoint.Y];
                    Point p2 = m_ItemLocationMatrixInPixel[keyPoint1.X, keyPoint1.Y];
                    Point p3 = m_ItemLocationMatrixInPixel[m_EndPoint.X, m_EndPoint.Y];
                    drawLine(p1, p2, p3);
                    m_SelectedCount = 0;
                    m_StartPoint = new Point();
                    m_EndPoint = new Point();
                }
                else if (Helpers.JudgeIn2RightAngle(m_StartPoint, m_EndPoint, m_ItemVisiabilityMatrixAroundTrue, out keyPoint1, out keyPoint2)
                    && startItem.m_Img.Equals(endItem.m_Img))
                {
                    m_GameMatrix[m_StartPoint.X - 1, m_StartPoint.Y - 1].Visible = false;
                    m_GameMatrix[m_EndPoint.X - 1, m_EndPoint.Y - 1].Visible = false;
                    m_ItemVisiabilityMatrixAroundTrue[m_StartPoint.X, m_StartPoint.Y] = 0;
                    m_ItemVisiabilityMatrixAroundTrue[m_EndPoint.X, m_EndPoint.Y] = 0;

                    string x1 = keyPoint1.X.ToString();
                    string y1 = keyPoint1.Y.ToString();
                    string x2 = keyPoint2.X.ToString();
                    string y2 = keyPoint2.Y.ToString();
                    ClearSelection();
                    ShowMatrix();
                    Point p1 = m_ItemLocationMatrixInPixel[m_StartPoint.X, m_StartPoint.Y];
                    Point p2 = m_ItemLocationMatrixInPixel[keyPoint1.X, keyPoint1.Y];
                    Point p3 = m_ItemLocationMatrixInPixel[keyPoint2.X, keyPoint2.Y];
                    Point p4 = m_ItemLocationMatrixInPixel[m_EndPoint.X, m_EndPoint.Y];
                    drawLine(p1, p2, p3, p4);
                    m_SelectedCount = 0;
                    m_StartPoint = new Point();
                    m_EndPoint = new Point();
                }
                else
                {
                    ClearSelection(m_EndPoint.X - 1, m_EndPoint.Y - 1);
                    ShowMatrix();
                }
                if (IsFinished())
                {
                    MessageBox.Show("You Win!");
                    Finished();
                }
                //Generate Matrix
            }
            else if (m_SelectedCount > 2)
            {
                ShowMatrix();
                ClearSelection();
                m_SelectedCount = 0;
            }
        }

        private bool IsFinished()
        {
            bool flag = false;
            for (int rowIndex = 0; rowIndex < m_ItemVisiabilityMatrixAroundTrue.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < m_ItemVisiabilityMatrixAroundTrue.GetLength(1); colIndex++)
                {
                    if (m_ItemVisiabilityMatrixAroundTrue[rowIndex, colIndex] == 1)
                    {
                        return flag;
                    }
                }
            }
            flag = true;
            return flag;
        }

        private void DisplayMatrixTest()
        {
            StringBuilder sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < m_ItemVisiabilityMatrixAroundTrue.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < m_ItemVisiabilityMatrixAroundTrue.GetLength(1); colIndex++)
                {
                    sb.Append(m_ItemVisiabilityMatrixAroundTrue[rowIndex, colIndex] + ",");
                }
                sb.AppendLine();
            }
            this.label1.Text = sb.ToString();
        }

        private void DisplayGameMatrixTest()
        {
            StringBuilder sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < m_GameMatrix.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < m_GameMatrix.GetLength(1); colIndex++)
                {
                    if (m_GameMatrix[rowIndex, colIndex].Visible)
                    {
                        sb.Append("1,");
                    }
                    else
                    {
                        sb.Append("0,");
                    }
                }
                sb.AppendLine();
            }
            this.label2.Text = sb.ToString();
        }

        private void ShowMatrix()
        {
            DisplayMatrixTest();
            DisplayGameMatrixTest();
        }

        private void drawLine(Point pStart, Point p_End)
        {
            gp = this.panel1.CreateGraphics();
            if (gp != null)
            {
                pStart.X += m_DrawOffSetX;
                pStart.Y += m_DrawOffSetY;
                p_End.X += m_DrawOffSetX;
                p_End.Y += m_DrawOffSetY;
                gp.DrawLine(blackPen, pStart, p_End);
                this.timer2.Interval = 500;
                this.timer2.Start();
            }
        }

        private void drawLine(Point pStart, Point keyPoint1, Point p_End)
        {
            gp = this.panel1.CreateGraphics();
            if (gp != null)
            {
                pStart.X += m_DrawOffSetX;
                pStart.Y += m_DrawOffSetY;
                keyPoint1.X += m_DrawOffSetX;
                keyPoint1.Y += m_DrawOffSetY;
                p_End.X += m_DrawOffSetX;
                p_End.Y += m_DrawOffSetY;
                gp.DrawLine(blackPen, pStart, keyPoint1);
                gp.DrawLine(blackPen, keyPoint1, p_End);
                this.timer2.Interval = 500;
                this.timer2.Start();
            }
        }

        private void drawLine(Point pStart, Point keyPoint1, Point keyPoint2, Point p_End)
        {
            gp = this.panel1.CreateGraphics();
            if (gp != null)
            {
                pStart.X += m_DrawOffSetX;
                pStart.Y += m_DrawOffSetY;
                keyPoint1.X += m_DrawOffSetX;
                keyPoint1.Y += m_DrawOffSetY;
                keyPoint2.X += m_DrawOffSetX;
                keyPoint2.Y += m_DrawOffSetY;
                p_End.X += m_DrawOffSetX;
                p_End.Y += m_DrawOffSetY;
                gp.DrawLine(blackPen, pStart, keyPoint1);
                gp.DrawLine(blackPen, keyPoint1, keyPoint2);
                gp.DrawLine(blackPen, keyPoint2, p_End);
                this.timer2.Interval = 500;
                this.timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (gp != null)
            {
                gp.Clear(Color.White);
                this.timer2.Stop();
            }
        }

        //Game time
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (GameTime > 0)
            {
                if (GameTime <= 20)
                {
                    this.label4.ForeColor = Color.Red;
                }
                this.label4.Text = GameTime.ToString();
                GameTime--;
            }
            else
            {
                this.label4.Text = GameTime.ToString();
                this.timer3.Stop();
                if (!IsFinished())
                {
                    MessageBox.Show("You lose!");
                    Finished();
                }
            }
        }

        //Help
        private void clickableLabel1_Click(object sender, EventArgs e)
        {
            ResortItems();
            ReDisplay();
        }

        //Easy
        private void clickableLabel2_Click(object sender, EventArgs e)
        {
            RowNum = 6;
            ColumnNum = 6;
            GameTime = GameTimeLevel1;
            MarginX = 10;
            MarginY = 10;
            m_CurrentLevel = 1;
            GroupSelect(this.clickableLabel2);
        }

        //Normal
        private void clickableLabel3_Click(object sender, EventArgs e)
        {
            RowNum = 6;
            ColumnNum = 10;
            GameTime = GameTimeLevel2;
            MarginX = 10;
            MarginY = 10;
            m_CurrentLevel = 2;

            GroupSelect(this.clickableLabel3);
        }

        //Hard
        private void clickableLabel4_Click(object sender, EventArgs e)
        {
            RowNum = 8;
            ColumnNum = 14;
            GameTime = GameTimeLevel3;
            MarginX = 10;
            MarginY = 10;
            m_CurrentLevel = 3;
            GroupSelect(this.clickableLabel4);
        }

        //Super
        private void clickableLabel5_Click(object sender, EventArgs e)
        {
            RowNum = 10;
            ColumnNum = 16;
            MarginX = 10;
            MarginY = 10;
            GameTime = GameTimeLevel4;
            m_CurrentLevel = 4;
            GroupSelect(this.clickableLabel5);
        }

        //To make sure just select one level 
        private void GroupSelect(ClickableLabel selectedLabel)
        {
            foreach (ClickableLabel label in m_MenuItems)
            {
                if (label.Text != selectedLabel.Text)
                {
                    label.IsSelected = false;
                    label.BackColor = label.PreviousColor;
                }
            }
        }

        private void GroupSelect()
        {
            foreach (ClickableLabel label in m_MenuItems)
            {
                label.IsSelected = false;
                label.BackColor = label.PreviousColor;
            }
        }

        private void setcolors()
        {
            this.clickableLabel2.BackColor = Color.Black;
        }

        //Start
        private void clickableLabel6_Click(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.panel1.Visible = true;
            this.panel3.Visible = true;
            InitializeGame();
            GroupSelect();
        }

        //Exit
        private void clickableLabel7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Menu
        private void clickableLabel8_Click(object sender, EventArgs e)
        {
            this.timer3.Stop();
            if (MessageBox.Show("Back to menu will lose your current game, continue?", 
                "Message", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                Finished();
            }
            else
            {
                this.timer3.Start();
            }
        }

        private void Finished()
        {
            this.panel1.Visible = false;
            this.label4.Visible = false;
            this.panel3.Visible = false;
            this.panel2.Visible = true;
            this.Width = 425;
            this.Height = 425;
        }
    }
}
