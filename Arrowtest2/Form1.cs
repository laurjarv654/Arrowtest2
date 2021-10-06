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
        //arrow x and y, and counter for when new arrows appear
        int X, Y, arrowSpace;
        int SIZE = 75, SPEED = 5;
        Image arrowImage;
        string arrowNum, space = "1";
        int lifePoints = 100, gainedPoints = 0, counter = 0;

        //2D list - list of lists
        List<List<Arrow>> arrows = new List<List<Arrow>>();

        //key press booleans
        Boolean upDown, downDown, rightDown, leftDown, enterDown;
        public Form1()
        {
            InitializeComponent();

            //Arrow list initialization
            X = 100;
            Y = this.Height+SIZE;

            //makes 4 lists in my 2D arrow list (for the 4 arrow directions)
            for (int i = 3; i >=0; i--)
            {
                arrows.Add(new List<Arrow>());
            }

            #region xml reading/arrow intitialization
            XmlReader reader = XmlReader.Create("Resources/arrows.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    //getting values from xml and setting them to variables
                    //xml edits aren't updating - "space" sibling doesn't exist
                    arrowNum = reader.Value;
                    reader.ReadToNextSibling("space");
                    space = reader.Value;
                    reader.ReadToNextSibling("number");

                    //arrowSpace = Convert.ToInt32(space);

                    //assigning the different arrows to their specific lists
                    switch (arrowNum)
                    {
                        case "0":
                            arrows[0].Add(new Arrow(X, Y, SIZE, 0, Properties.Resources.arrow0));
                            break;
                        case "1":
                            arrows[1].Add(new Arrow(X + 100, Y + 100, SIZE, 1, Properties.Resources.arrow1));
                            break;
                        case "2":
                            arrows[2].Add(new Arrow(X + 200, Y + 200, SIZE, 2, Properties.Resources.arrow2));
                            break;
                        case "3":
                            arrows[3].Add(new Arrow(X + 300, Y + 300, SIZE, 3, Properties.Resources.arrow3));
                            break;
                    }
                }
            }
            reader.Close();
            #endregion


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
                foreach (Arrow a in arrows[i])
                {
                    a.y -= SPEED;

                }
            }

            //check if there's anything in arrows list first and if 
            //if (arrows.Any()&&arrows[0][0].y < 0)
            //{
            //    arrows.RemoveAt(0);
            //}


            //counter++;
            #endregion

            #region collision checks
            //if (leftDown == true)
            //{
            //    foreach (Arrow a in arrows[0])
            //    {
            //        if (a.y <= 10&& a.y>=10+a.size)
            //        {
            //            //alter image to lighter version
            //            gainedPoints += 100;
            //        }
            //        else if (counter >= 15)
            //        {
            //            //less points
            //            lifePoints -= 5;
            //            counter = 0;
            //        }
            //    }
            //}
            //if (upDown == true)
            //{
            //    foreach (Arrow a in arrows[1])
            //    {
            //        if (a.y <= 10 && a.y >= 10 + a.size)
            //        {
            //            gainedPoints += 100;
            //        }
            //        else if (counter >= 15)
            //        {
            //            lifePoints -= 5;
            //            counter = 0;
            //        }
            //    }
            //}
            //if (downDown == true)
            //{
            //    foreach (Arrow a in arrows[2])
            //    {
            //        if (a.y <= 10 && a.y >= 10 + a.size)
            //        {
            //            gainedPoints += 100;
            //        }
            //        else if(counter >= 15)
            //        {
            //            lifePoints -= 5;
            //            counter = 0;
            //        }
            //    }
            //}
            //if (rightDown == true)
            //{
            //    foreach (Arrow a in arrows[3])
            //    {
            //        if (a.y <= 10 && a.y >= 10 + a.size)
            //        {
            //            gainedPoints += 100;
            //        }
            //        else if (counter >= 15)
            //        {
            //            lifePoints -= 5;
            //            counter = 0;
            //        }
            //    }
            //}
            #endregion

            //testLabel.Text = Convert.ToString(gainedPoints);

            counter++;
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            #region drawing grey arrows
            e.Graphics.DrawImage(Properties.Resources.arrow0G, X, 10, SIZE, SIZE);
            e.Graphics.DrawImage(Properties.Resources.arrow1G, X+100, 10, SIZE, SIZE);
            e.Graphics.DrawImage(Properties.Resources.arrow2G, X+200, 10, SIZE, SIZE);
            e.Graphics.DrawImage(Properties.Resources.arrow3G, X+300, 10, SIZE, SIZE);
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

        }


    }
}
