using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Arrowtest2
{
    public partial class Form1 : Form
    {
        //arrow variables
        int X, Y, arrowSpace, SIZE = 75, SPEED = 8;
        string arrowNum, space = "1";

        //score variables
        int lifePoints = 100, gainedPoints = 0, counter = 0;

        //2D list - list of lists
        List<List<Arrow>> arrows = new List<List<Arrow>>();
        List<List<Rectangle>> arrowRec = new List<List<Rectangle>>();

        List<Arrow> greyArrows = new List<Arrow>();
        List<Rectangle> greyRec = new List<Rectangle>();

        //key press booleans
        Boolean upDown, downDown, rightDown, leftDown, enterDown;

        public Form1()
        {
            InitializeComponent();

            ArrowInitialization();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                case Keys.Enter:
                    enterDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
                case Keys.Enter:
                    enterDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            #region arrow movement
            //move each arrow
            for (int i = 0; i<=arrows.Count()-1; i++)
            {
                for (int j = 0; j<= arrows[i].Count()-1; j++)
                {
                    arrows[i][j].y -= SPEED;
                    //arrowRec[i][j] = new Rectangle(arrows[i][j].x, arrows[i][j].y, SIZE, SIZE);

                }
            }

            //updating the rectangles
            for (int i = 0; i <= arrows.Count() - 1; i++)
            {
                arrowRec[i].Clear();
                
                foreach (Arrow a in arrows[i])
                {
                    Rectangle tempRec = new Rectangle(a.x, a.y, SIZE, SIZE);
                    arrowRec[i].Add(tempRec);
                }
                
            }

            //check if there's anything in arrows list first and if 
            //if (arrows.Any()&&arrows[0][0].y < 0)
            //{
            //    arrows.RemoveAt(0);
            //}


            #endregion

            Collisions();

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            #region drawing grey arrows
           foreach (Arrow a in greyArrows)
            {
                e.Graphics.DrawImage(a.getImage(), a.x, a.y, SIZE, SIZE);       
            }
            #endregion

            #region drawing regular arrows
            for (int i = 0; i <= arrows.Count() - 1; i++)
            {
                foreach (Arrow a in arrows[i])
                {
                    //drawing the arrows
                    e.Graphics.DrawImage(a.getImage(), a.x, a.y - (SIZE * 2), SIZE, SIZE);
                }
            }
            #endregion

            //drawing rectangles for testing
            Pen pen = new Pen(Color.Red);
            foreach(List<Rectangle> l in arrowRec)
            {
                foreach (Rectangle r in l)
                {
                    e.Graphics.DrawRectangle(pen, r);
                }
            }

            foreach (Rectangle r in greyRec)
            {
                e.Graphics.DrawRectangle(pen, r);
            }
        }


        public void ArrowInitialization()
        {
            //Arrow list initialization
            X = 100;
            Y = this.Height + SIZE;

            #region moving arrows
            //makes 4 lists in my 2D arrow list and arrow rectangles(for the 4 arrow directions)
            for (int i = 3; i >= 0; i--)
            {
                arrows.Add(new List<Arrow>());
                arrowRec.Add(new List<Rectangle>());
            }

            //pulling the arrow information from an xml
            XmlReader reader = XmlReader.Create("Resources/arrows.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    //getting values from xml and setting them to variables
                    arrowNum = reader.ReadString();
                    reader.ReadToNextSibling("space");
                    space = reader.ReadString();

                    arrowSpace = Convert.ToInt32(space);

                    //assigning the different arrows to their specific lists
                    switch (arrowNum)
                    {
                        case "0":
                            arrows[0].Add(new Arrow(X, Y + arrowSpace, 0, Properties.Resources.arrow0));
                            break;
                        case "1":
                            arrows[1].Add(new Arrow(X + 100, Y + arrowSpace, 1, Properties.Resources.arrow1));
                            break;
                        case "2":
                            arrows[2].Add(new Arrow(X + 200, Y + arrowSpace, 2, Properties.Resources.arrow2));
                            break;
                        case "3":
                            arrows[3].Add(new Arrow(X + 300, Y + arrowSpace, 3, Properties.Resources.arrow3));
                            break;
                    }
                }
            }
            #endregion

            #region grey arrows
            //filling the grey arrows list 
            greyArrows.Add(new Arrow(X, 10, 0, Properties.Resources.arrow0G));
            greyArrows.Add(new Arrow(X + 100, 10, 0, Properties.Resources.arrow1G));
            greyArrows.Add(new Arrow(X + 200, 10, SIZE, Properties.Resources.arrow2G));
            greyArrows.Add(new Arrow(X + 300, 10, SIZE, Properties.Resources.arrow3G));

            //setting up the grey arrow rectangles
            foreach (Arrow a in greyArrows)
            {
                greyRec.Add(new Rectangle(a.x, a.y, SIZE, SIZE));
            }
            #endregion
        }

        public void Collisions ()
        {
            if (leftDown == true)
            {
                for (int i = 0; i<=arrows[0].Count()-1; i++)
                {
                    if (arrowRec[0][i].IntersectsWith(greyRec[0])&&counter>=15)
                    {
                        //alter image to lighter version
                        gainedPoints += 100;
                        arrows[0][i].setImage(Properties.Resources.arrow0W);
                        counter = 0;
                    }
                    else if (counter >= 15)
                    {
                        //less points
                        lifePoints -= 5;
                        counter = 0;
                    }
                }
            }
            if (upDown == true)
            {
                for (int i = 0; i <= arrows[1].Count() - 1; i++)
                {
                    if (arrowRec[1][i].IntersectsWith(greyRec[1])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[1][i].setImage(Properties.Resources.arrow1W);
                        counter = 0;
                    }
                    else if (counter >= 15)
                    {
                        lifePoints -= 5;
                        counter = 0;
                    }
                }
            }
            if (downDown == true)
            {
                for (int i = 0; i<= arrows[2].Count()-1; i++)
                {
                    if (arrowRec[2][i].IntersectsWith(greyRec[2])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[2][i].setImage(Properties.Resources.arrow2W);
                        counter = 0;
                    }
                    else if (counter >= 15)
                    {
                        lifePoints -= 5;
                        counter = 0;
                    }
                }
            }
            if (rightDown == true)
            {
                for (int i = 0; i<=arrows[3].Count()-1; i++)
                {
                    if (arrowRec[3][i].IntersectsWith(greyRec[3])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[3][i].setImage(Properties.Resources.arrow3W);
                        counter = 0;
                    }
                    else if (counter >= 15)
                    {
                        lifePoints -= 5;
                        counter = 0;
                    }
                }
            }
            counter++;
            testLabel.Text = Convert.ToString(gainedPoints);
        }

    }
}
