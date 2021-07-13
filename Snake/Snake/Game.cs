﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public class Game
    {
        public int scale = 10;
        public int lengthMap = 30;
        private int[,] squares;
        private List<Square> Snake;

        public enum Direction { Right, Down, Left, Up }
        public Direction ActualDirection = Direction.Right;
        private Square Food = null;
        private Random oRandom = new Random();
        private int Points = 0;

        PictureBox oPictureBox;
        Label lblPuntos;

        private int InitialPositionX
        {
            get {
                return lengthMap / 2;
            }
        } 

        private int InictialPositionY
        {
            get
            {
                return lengthMap / 2;
            }
        }

        public bool IsLost
        {
            get 
            {
                foreach (var oSquare in Snake)
                {
                    if (Snake.Where(d=> d.Y==oSquare.Y & d.X==oSquare.X && oSquare!=d).Count() > 0)
                    {
                        return true;
                    }
                    

                }
                return false;
            }
        }

        public Game(PictureBox oPictureBox, Label lblPuntos) {
            
            this.oPictureBox = oPictureBox;
            this.lblPuntos = lblPuntos;
            Reset();

        }

        public void Reset() 
        {
            Snake = new List<Square>();
            Square oInitialSquare = new Square(InitialPositionX, InictialPositionY);
            Snake.Add(oInitialSquare);
            squares = new int[lengthMap,lengthMap];
            for (int j = 0;j < lengthMap; j++)
            {
                for (int i = 0; i < lengthMap; i++)
                {
                    squares[i, j] = 0;
                }
            }
            Points = 0;
        }
        public void Show()
        {
            Bitmap bmp = new Bitmap(oPictureBox.Width, oPictureBox.Height);

            for (int j = 0; j < lengthMap; j++)
            {
                for (int i = 0; i < lengthMap; i++)
                {
                    if (Snake.Where(d => d.X == i && d.Y == j).Count() > 0)
                    {
                        PaintPixel(bmp, i, j, Color.Black);

                    }

                    else
                    {
                        PaintPixel(bmp, i, j, Color.White);
                    }
                }
            }
            if (Food!=null)
            {
                PaintPixel(bmp, Food.X, Food.Y, Color.Green);
            }
            


            oPictureBox.Image = bmp;
            this.lblPuntos.Text = Points.ToString();
        }

        public void Next() {

            if (Food == null)
            {
                GetFood();
            }

            GetHistorySnake();

            switch (ActualDirection)
            {
                case Direction.Right:
                    {
                        if (Snake[0].X ==(lengthMap - 1))
                        
                            Snake[0].X = 0;
                        
                        else
                        
                            Snake[0].X++;
                        break;

                    }
                case Direction.Left:
                    {
                        if (Snake[0].X == 0)

                            Snake[0].X = lengthMap - 1;

                        else

                            Snake[0].X--;
                        break;

                    }
                case Direction.Up:
                    {
                        if (Snake[0].Y == (lengthMap - 1))

                            Snake[0].Y = 0;

                        else

                            Snake[0].Y++;
                        break;

                    }
                case Direction.Down:
                    {
                        if (Snake[0].Y == 0)

                            Snake[0].Y = (lengthMap - 1);

                        else

                            Snake[0].Y--;
                        break;

                    }
            }

            GetNextMoveSnake();
            SnakeEating();

        }

        private void GetNextMoveSnake() 
        {
            if (Snake.Count>1)
            {
                for (int i = 1; i < Snake.Count; i++)
                {
                    Snake[i].X = Snake[i - 1].X_old;
                    Snake[i].Y = Snake[i - 1].Y_old;
                }
            }
        }
        private void GetHistorySnake() 
        {
            foreach (var oSquare in Snake)
            {
                oSquare.X_old = oSquare.X;
                oSquare.Y_old = oSquare.Y;
            }
        }
        private void SnakeEating() 
        {
            if (Snake[0].X == Food.X && Snake[0].Y == Food.Y)
            {
                Food = null;
                Points++;

                //asignar nuevo elemento al snake
                Square lastSquare = Snake[Snake.Count - 1];
                Square oSquare = new Square(lastSquare.X_old,lastSquare.Y_old);
                Snake.Add(oSquare);
            }
            
        }
        private void GetFood() 
        {
            int X = oRandom.Next(0, lengthMap - 1);
            int Y = oRandom.Next(0, lengthMap - 1);

            Food = new Square(X, Y);
        }
        private void PaintPixel(Bitmap bmp, int x, int y, Color color)
        {
            for (int j = 0; j < scale; j++)
            {
                for (int i = 0; i < scale; i++)
                {
                    bmp.SetPixel(i+(x*scale), j+(y*scale),color) ;


                }
            }
        }
    }

   

    public class Square 
    {
        public int X, Y, X_old, Y_old;
        public Square(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
            this.X_old = X;
            this.Y_old = Y;
        }
    }
}
