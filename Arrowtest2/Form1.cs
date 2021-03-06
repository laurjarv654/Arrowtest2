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
        #region global variables
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
        #endregion

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
            for (int i = 0; i<arrows.Count(); i++)
            {
                for (int j = 0; j<arrows[i].Count(); j++)
                {
                    arrows[i][j].y -= SPEED;
                   
                }
            }

            //updating the rectangles
            for (int i = 0; i <arrows.Count(); i++)
            {
                arrowRec[i].Clear();

                for (int j = 0; j<arrows[i].Count(); j++)
                {
                    Rectangle tempRec = new Rectangle(arrows[i][j].x, arrows[i][j].y, SIZE, SIZE);
                    arrowRec[i].Add(tempRec);
                }
                
            }

            //removes arrows that go off the screen 
            for (int i = 0; i<arrows.Count();i++)
            {
                if (arrows[i].Count() > 0 && arrows[i][0].y + SIZE < 0)
                {
                    arrows[i].RemoveAt(0);
                }
            }
            


            #endregion

            Collisions();

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            #region drawing grey arrows
           for (int i = 0; i< greyArrows.Count(); i++)
            {
                e.Graphics.DrawImage(greyArrows[i].getImage(), greyArrows[i].x, greyArrows[i].y, SIZE, SIZE);       
            }
            #endregion

            #region drawing regular arrows
            for (int i = 0; i < arrows.Count(); i++)
            {
                for (int j = 0; j<arrows[i].Count(); j++)
                {
                    //drawing the arrows
                    e.Graphics.DrawImage(arrows[i][j].getImage(), arrows[i][j].x, arrows[i][j].y, SIZE, SIZE);
                }
            }
            #endregion

            //drawing rectangles for testing
            Pen pen = new Pen(Color.Red);
            for(int i = 0; i<arrowRec.Count();i++)
            {
                for (int j = 0; j<arrowRec[i].Count(); j++)
                {
                    e.Graphics.DrawRectangle(pen, arrowRec[i][j]);
                }
            }

            for (int i = 0; i<greyRec.Count(); i++)
            {
                e.Graphics.DrawRectangle(pen, greyRec[i]);
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
            for (int i = 0; i<greyArrows.Count(); i++)
            {
                greyRec.Add(new Rectangle(greyArrows[i].x, greyArrows[i].y, SIZE, SIZE));
            }
            #endregion
        }

        public void Collisions ()
        {
            if (leftDown == true)
            {
                for (int i = 0; i<arrows[0].Count(); i++)
                {
                    if (arrowRec[0][i].IntersectsWith(greyRec[0])&&counter>=15)
                    {
                        //alter image to lighter version
                        gainedPoints += 100;
                        arrows[0][i].setImage(Properties.Resources.arrow0W);
                        arrows[0].RemoveAt(i);
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
                for (int i = 0; i<arrows[1].Count(); i++)
                {
                    if (arrowRec[1][i].IntersectsWith(greyRec[1])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[1][i].setImage(Properties.Resources.arrow1W);
                        arrows[1].RemoveAt(i);
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
                for (int i = 0; i<arrows[2].Count(); i++)
                {
                    if (arrowRec[2][i].IntersectsWith(greyRec[2])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[2][i].setImage(Properties.Resources.arrow2W);
                        arrows[2].RemoveAt(i);
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
                for (int i = 0; i<arrows[3].Count(); i++)
                {
                    if (arrowRec[3][i].IntersectsWith(greyRec[3])&&counter>=15)
                    {
                        gainedPoints += 100;
                        arrows[3][i].setImage(Properties.Resources.arrow3W);
                        arrows[3].RemoveAt(i);
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
            //shows points for testing purposes
            testLabel.Text = "G:"+Convert.ToString(gainedPoints)+"       L:"+Convert.ToString(lifePoints);
        }

    }
}
