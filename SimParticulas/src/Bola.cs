﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity_Simulation
{
    public class Ballinstance
    {
        int xpos, ypos;
        const int ground = 500;

        public Ballinstance()
        {
        }


        public void play(ref double xspeed, ref double yspeed, ref double newyspeed, ref double startingypos, ref double newxpos, ref double newypos, ref double oldxpos, ref double oldypos, ref double newx, ref double oldx, ref double newy, ref double oldy, ref double acc, ref double t, ref int xmouse, ref int ymouse, ref bool dragging, ref bool trace, ref bool collisiony)
        {

            xpos = (int)newxpos;
            ypos = (int)newypos;

            // This code will be visited 50 times per second while dragging
            if (dragging)
            {
                // Grip the center of the ball when dragging
                xpos = xmouse;
                ypos = ymouse;

                // While dragging the starting y-axis position of the ball is ball.Top
                startingypos = ground - ypos;

                // Calculate the x and y speed based on the mouse movement within 20 msec
                // speed = distance/time  ->  time = 20 millisecond
                // the speed is the change in the displacement with respect to time which
                // is already running (the code is within the timer), so we don't have to divide 
                // by time.
                newx = xpos;
                newy = ground - ypos;
                xspeed = (newx - oldx) / 1;
                yspeed = (newy - oldy) / 1;
                oldx = newx;
                oldy = newy;

                // The time -while dragging- will not start yet
                t = 0;

            }
            else
            {
                /*if(trace)
                {
                    Size s = new Size(6,6);
                    Point p = new Point(ball.Right,ball.Bottom);
                    Rectangle r = new Rectangle(p,s);
                    Pen pen = new Pen(Color.Blue,1);
                    Graphics graphics = CreateGraphics();
                    graphics.DrawRectangle(pen,r);
                }*/

                // This code will be visited 50 times per second while not dragging
                // The ball position is where it's last dragged
                oldxpos = xpos;
                // X-axis motion
                if (xpos < 580 && 0 < xpos)
                {
                    newxpos = oldxpos + xspeed;
                }
                else
                {
                    // Here the ball will hits the wall
                    // Ball xspeed will decrease every time it hits the wall
                    // Minus sign: to change the ball direction when it collides with the wall
                    xspeed *= -0.9;	// Wall resistance	
                    newxpos = oldxpos + xspeed;
                }

                // Y-axis motion
                if (0 < newypos || collisiony)
                {
                    newyspeed = yspeed - (acc * t);
                    newypos = startingypos + ((yspeed * t) - 0.5 * acc * (t * t));
                    collisiony = false;
                }
                else
                {
                    // Here the ball will hits the ground
                    // Initialize the ball variables again
                    startingypos = -1;
                    // Here set startingypos=-1 not 0, because if 0 newypos will be 0 every time the ball 
                    // Hits the ground so no bouncing will happens to the ball, evaluate to the 
                    // eguation below for newypos when t = 0
                    t = 0;
                    // Ball yspeed will decrease every time it hits the ground
                    // 0.75 is the elasticity coefficient - assumption
                    // The initial speed(yspeed) is 0.75 of the final speed(newyspeed)
                    yspeed = newyspeed * -0.75;
                    newypos = startingypos + ((yspeed * t) - 0.5 * acc * (t * t));
                    collisiony = true;
                }
                // Always
                // Ball xspeed will always decrease, even if it didn't hit the wall
                xspeed *= 0.99;	// Air resistance

                if (xspeed > -0.5 && xspeed < 0)
                    xspeed = 0;

                #region Explination of xspeed condition above
                // This condition is to stop the ball when it heading to the left, you can notice that removeing
                // this condition will make the ball never stop while its heading to the left until it will
                // hit the left wall, to know why, run the simulation under the debuging mode and watch
                // the value of newxpos
                // newxpos = oldxpos + xspeed
                // when 0 < xspeed < 1 (the ball heading right), ball.left = (int)newxpos, the casting 
                // forces the ball left position value to be the same as its previous value, because oldxpos and newxpos are equals, and hence the ball will stop.
                // but when -1 < xspeed < 0 (the ball heading left), ball.left = (int)newxpos, the casting
                // here will not work correctly, because the value of oldxpos(which is integer value see line 185) will always 
                // be decremented by the xspeed, this will force the newxpos also to be always decremented by xspeed and hence
                // ball.left will always decremented by 1 (int) casting, and hence the ball will never stop.
                #endregion

                // Update the ball position
                xpos = (int)newxpos;
                ypos = (int)(ground - newypos);
                // Increase the time
                t += 0.3;
            }
        }

    }
}
