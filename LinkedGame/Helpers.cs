using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
namespace LinkedGame
{
    public delegate void SelectHandler(object obj,EventArgs args);
    //public delegate void DoSomethingInLoopDelegate(int currentRowIndex, int currentColumnIndex,object obj);
    class Helpers
    {
        public static string[] GetImageFiles(string subFolder)
        {
            try
            {
                string filePath = System.Environment.CurrentDirectory + @"\images\";
                if (subFolder.Length > 0)
                {
                    filePath = System.Environment.CurrentDirectory + @"\images\" + subFolder + @"\";
                }
                DirectoryInfo di = new DirectoryInfo(filePath);
                FileInfo[] fiList = di.GetFiles();
                string[] resultList = new string[fiList.Count<FileInfo>()];
                for (int index = 0; index < resultList.Count<string>(); index++)
                {
                    if (fiList[index].Extension.ToLower().Equals(".jpg") ||
                        fiList[index].Extension.ToLower().Equals(".png") ||
                        fiList[index].Extension.ToLower().Equals(".gif"))
                    {
                        resultList[index] = fiList[index].FullName;
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static string[] GetImageSubFolders(string filePath)
        {
            try
            {
                //string filePath = System.Environment.CurrentDirectory + @"\images\";
               
                DirectoryInfo di = new DirectoryInfo(filePath);
                DirectoryInfo[] diList = di.GetDirectories();
                string[] resultList = new string[diList.Count<DirectoryInfo>()];
                for (int index = 0; index < resultList.Count<string>(); index++)
                {
                    resultList[index] = diList[index].Name;
                }
                return resultList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static void RandomSort(object[] array)
        {
            object temp;
            int index0;
            Random ra = new Random();
            for (int i = 1; i < array.Length; i++)
            {
                index0 = ra.Next(i);
                temp = array[index0];
                array[index0] = array[i];
                array[i] = temp;
            }
        }

        public static void RadmonSort(object[,] array)
        {
            //1.Initialize arrayInALine
            object[] arrayInALine = new object[array.Length];
            int currentIndex = 0;
            for (int rowIndex = 0; rowIndex < array.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < array.GetLength(1); columnIndex++)
                {
                    arrayInALine[currentIndex] = array[rowIndex, columnIndex];
                    currentIndex++;
                }
            }
            currentIndex = 0;
            //2.Radom sort it
            RandomSort(arrayInALine);
            //3.Set values back
            for (int rowIndex = 0; rowIndex < array.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < array.GetLength(1); columnIndex++)
                {
                    array[rowIndex, columnIndex] = arrayInALine[currentIndex];
                    currentIndex++;
                }
            }
        }

        public static bool JudgeIn2RightAngle(Point start, Point end, int[,] Matrix, out Point keyPoint1, out Point keyPoint2)
        {
            bool resultFlag = false;
            bool swapFlag = false;
            Point tempPointForSwap = new Point(0, 0);
            Point ptLeft = new Point(0, 0);
            Point ptRight = new Point(0, 0);
            keyPoint1 = new Point(0, 0);
            keyPoint2 = new Point(0, 0);
            //Make sure start point's X is lower
            if (start.X > end.X)
            {
                tempPointForSwap = start;
                start = end;
                end = tempPointForSwap;
                swapFlag = true;
            }

            //1.Generate aviliable Points of start
            List<Point> aviliablePoints = new List<Point>();
            //------------------------>X
            //|  xx  xx    xx   xx
            //|  xx  start xx   xx
            //|  xx  xx    end  xx
            //|  xx  xx    xx   xx
            //|
            //Y
            //down, right, up, left
            if (start.Y < end.Y)
            {
                //down 
                for (int rowIndex = start.X; rowIndex < Matrix.GetLength(0); rowIndex++)
                {
                    if (Matrix[rowIndex, start.Y] == 0)
                    {
                        aviliablePoints.Add(new Point(rowIndex, start.Y));
                    }
                }
                //right 
                for (int colIndex = start.Y; colIndex < Matrix.GetLength(1); colIndex++)
                {
                    if (Matrix[start.X, colIndex] == 0)
                    {
                        aviliablePoints.Add(new Point(start.X, colIndex));
                    }
                }
                //up
                for (int rowIndex = start.X; rowIndex >= 0; rowIndex--)
                {
                    if (Matrix[rowIndex, start.Y] == 0)
                    {
                        aviliablePoints.Add(new Point(rowIndex, start.Y));
                    }
                }
                //left
                for (int colIndex = start.Y; colIndex >= 0; colIndex--)
                {
                    if (Matrix[start.X, colIndex] == 0)
                    {
                        aviliablePoints.Add(new Point(start.X, colIndex));
                    }
                }
            }
            //------------------------>X
            //|  xx  xx    xx    xx
            //|  xx  xx   start  xx
            //|  xx  end   xx    xx
            //|  xx  xx    xx    xx
            //Y
            //left, down, right, up
            else
            {
                //Judge points inside rectangle first.
                //left
                for (int colIndex = start.Y; colIndex >= 0; colIndex--)
                {
                    if (Matrix[start.X, colIndex] == 0)
                    {
                        aviliablePoints.Add(new Point(start.X, colIndex));
                    }
                }
                //down
                for (int rowIndex = start.X; rowIndex < Matrix.GetLength(0); rowIndex++)
                {
                    if (Matrix[rowIndex, start.Y] == 0)
                    {
                        aviliablePoints.Add(new Point(rowIndex, start.Y));
                    }
                }
                //up
                for (int rowIndex = start.X; rowIndex >= 0; rowIndex--)
                {
                    if (Matrix[rowIndex, start.Y] == 0)
                    {
                        aviliablePoints.Add(new Point(rowIndex, start.Y));
                    }
                }
                //right
                for (int colIndex = start.Y; colIndex < Matrix.GetLength(1); colIndex++)
                {
                    if (Matrix[start.X, colIndex] == 0)
                    {
                        aviliablePoints.Add(new Point(start.X, colIndex));
                    }
                }
            }

            //Remove points can't access start point in line.
            List<Point> tempAviliablePoints = new List<Point>();
            tempAviliablePoints = aviliablePoints.ToList<Point>();
            foreach (Point point in tempAviliablePoints)
            {
                if (!JudgeInLine(start, point, Matrix))
                {
                    aviliablePoints.Remove(point);
                }
            }

            //Loop judge
            if (aviliablePoints.Count <= 0)
            {
                keyPoint1 = new Point(0, 0);
                keyPoint2 = new Point(0, 0);
                return resultFlag;
            }

            for (int i = 0; i < aviliablePoints.Count; i++)
            {
                Point keyPoint = new Point();
                if (JudgeInRightAngle(aviliablePoints[i], end, Matrix, out keyPoint))
                {
                    resultFlag = true;
                    if (swapFlag)
                    {
                        keyPoint2 = aviliablePoints[i];
                        keyPoint1 = keyPoint;
                    }
                    else
                    {
                        keyPoint1 = aviliablePoints[i];
                        keyPoint2 = keyPoint;
                    }
                    break;
                }
            }
            return resultFlag;
        }

        public static bool JudgeInRightAngle(Point start, Point end, int[,] Matrix, out Point keyPoint)
        {
            bool resultFlag = false;
            Point tempPointForSwap = new Point(0, 0);
            Point ptLeft = new Point(0, 0);
            Point ptRight = new Point(0, 0);
            //Make sure start point's X is lower
            if (start.X > end.X)
            {
                tempPointForSwap = start;
                start = end;
                end = tempPointForSwap;
            }
            //------------------------>X
            //|  xx  xx    xx   xx
            //|  xx  start xx   xx
            //|  xx  xx    end  xx
            //|  xx  xx    xx   xx
            //|
            //Y
            if (start.Y < end.Y)
            {
                ptLeft = new Point(end.X, start.Y);
                ptRight = new Point(start.X, end.Y);
                if (JudgeInLine(start, ptLeft, Matrix) && JudgeInLine(ptLeft, end, Matrix) && Matrix[ptLeft.X, ptLeft.Y] == 0)
                {
                    keyPoint = ptLeft;
                    resultFlag = true;
                }
                else if (JudgeInLine(start, ptRight, Matrix) && JudgeInLine(ptRight, end, Matrix) && Matrix[ptRight.X, ptRight.Y] == 0)
                {
                    keyPoint = ptRight;
                    resultFlag = true;
                }
                else
                {
                    keyPoint = new Point(-1, -1);
                }
            }

            //------------------------>X
            //|  xx  xx    xx    xx
            //|  xx  xx   start  xx
            //|  xx  end   xx    xx
            //|  xx  xx    xx    xx
            //Y
            else
            {
                ptLeft = new Point(start.X, end.Y);
                ptRight = new Point(end.X, start.Y);
                if (JudgeInLine(ptLeft, start, Matrix) & JudgeInLine(ptLeft, end, Matrix) && Matrix[ptLeft.X, ptLeft.Y] == 0)
                {
                    keyPoint = ptLeft;
                    resultFlag = true;
                }
                else if (JudgeInLine(end, ptRight, Matrix) & JudgeInLine(start, ptRight, Matrix) && Matrix[ptRight.X, ptRight.Y] == 0)
                {
                    keyPoint = ptRight;
                    resultFlag = true;
                }
                else
                {
                    keyPoint = new Point(-1, -1);
                }
            }
            return resultFlag;
        }

        public static bool JudgeInLine(Point start, Point end, int[,] Matrix)
        {
            bool flag = false;
            //1.return if invalid
            if ((start.X != end.X && start.Y != end.Y) || start.Equals(end))
            {
                return flag;
            }
            //2.swap if start > end then process it.
            if (start.X == end.X)
            {
                Point tempPointForSwap = new Point(0, 0);
                if (start.Y > end.Y)
                {
                    tempPointForSwap = start;
                    start = end;
                    end = tempPointForSwap;
                }
                for (int indexY = start.Y + 1; indexY < end.Y; indexY++)
                {
                    if (Matrix[start.X, indexY] != 0)
                    {
                        return flag;
                    }
                }
                flag = true;
            }
            else if (start.Y == end.Y)
            {
                Point tempPointForSwap = new Point(0, 0);
                if (start.X > end.X)
                {
                    tempPointForSwap = start;
                    start = end;
                    end = tempPointForSwap;
                }

                for (int indexX = start.X + 1; indexX < end.X; indexX++)
                {
                    if (Matrix[indexX, start.Y] != 0)
                    {
                        return flag;
                    }
                }
                flag = true;
            }
            return flag;
        }

        public static Image ResizeImageWithoutRatio(Image imgToResize, Size size)
        {
            int destWidth = size.Width;
            int destHeight = size.Height;

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
