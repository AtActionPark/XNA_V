using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace V
{
    public class Obstacle
    {
        Vector2 position;
        Vector2 velocity;
        Vector2 acceleration;


        public float angle = 0;
        Vector2 size;
        public Vector2 p1, p2, p3, p4;

        public bool alive = true;
        public Color color;
        Random random = new Random();

        float transition;
        public float framesToStart;
        public bool changeSize = false;
        public bool followLine = false;

        #region special object
        public bool isSpecial = false;
        float specialSpeed = 0;
        public float specialAcc = 0.05f;
        public Direction specialDirection = Direction.none;
        bool moveLeft = true;
        bool moveRight = true;
        bool moveDown = true;
        public bool move = false;
        #endregion

        public Obstacle(Vector2 position, Vector2 acceleration, Vector2 size)
        {
            this.position = position;
            this.acceleration = acceleration;
            this.size = size;

            //set 4 points with size and position
            p1 = new Vector2(position.X - size.X / 2, position.Y - size.Y / 2);
            p2 = new Vector2(position.X + size.X / 2, position.Y - size.Y / 2);
            p3 = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            p4 = new Vector2(position.X - size.X / 2, position.Y + size.Y / 2);

            //set rotation matrix
            Matrix m = Matrix.CreateRotationZ(angle);
            Matrix t = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            Matrix t2 = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
            Matrix end = t2 * m * t;

            //set point position with angle
            p1 = Vector2.Transform(p1, end);
            p2 = Vector2.Transform(p2, end);
            p3 = Vector2.Transform(p3, end);
            p4 = Vector2.Transform(p4, end);
        }

        public void Update()
        {
            if (transition < 1)
                transition += 0.05f;

            framesToStart--;

            #region changeSize
            if (changeSize)
            {
                if (specialDirection == Direction.down)
                {
                    if (size.Y < 10)
                        size.Y -= 5;
                    if (size.X <400 )
                        size.X += 3;
                }
                else
                {
                    if (size.Y > 10)
                        size.Y -= 5;
                    if (size.X < 600)
                        size.X += 5;
                }
            }
            #endregion

            if (framesToStart <= 0)
            {
                #region follow line
                if (followLine)
                {
                    if (specialDirection == Direction.left)
                        if (position.X <= V.Instance.point2.X)
                            acceleration = new Vector2(V.Instance.point2.X - position.X, V.Instance.point2.Y - position.Y);
                    else
                        if (position.X >= V.Instance.point4.X)
                            acceleration = new Vector2(V.Instance.point4.X - position.X, V.Instance.point4.Y - position.Y);
                    acceleration.Normalize();
                    if ((specialDirection == Direction.left && position.X > V.Instance.point2.X) ||(specialDirection == Direction.right&& position.X < V.Instance.point4.X))
                    {
                        if(specialDirection == Direction.left)
                            velocity = new Vector2(10, 0)  *GamePlayState.velocityMultiplier;
                        else
                            velocity = new Vector2(-10, 0) * GamePlayState.velocityMultiplier;
                        position += velocity;
                    }
                    else
                    {
                        acceleration /= 50;
                        position += velocity * GamePlayState.velocityMultiplier;
                        velocity += acceleration;
                        FollowLine();
                    }
                }
                #endregion

                position += velocity*GamePlayState.velocityMultiplier;
                velocity += acceleration;
            }

            #region set points position
            p1 = new Vector2(position.X - size.X / 2, position.Y - size.Y / 2);
            p2 = new Vector2(position.X + size.X / 2, position.Y - size.Y / 2);
            p3 = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            p4 = new Vector2(position.X - size.X / 2, position.Y + size.Y / 2);

            Matrix m = Matrix.CreateRotationZ(angle);
            Matrix t = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            Matrix t2 = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
            Matrix end = t2*m*t;

            p1 = Vector2.Transform(p1, end);
            p2 = Vector2.Transform(p2, end);
            p3 = Vector2.Transform(p3, end);
            p4 = Vector2.Transform(p4, end);
            #endregion

            if (isOutOfScreen())
                alive = false;

            if (isSpecial)
            {
                SpecialUpdate();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Primitive.DrawLineSegment(spriteBatch, p1, p2, Color.Lerp(Color.Black, color, transition), 3);
            Primitive.DrawLineSegment(spriteBatch, p2, p3, Color.Lerp(Color.Black, color, transition), 3);
            Primitive.DrawLineSegment(spriteBatch, p3, p4, Color.Lerp(Color.Black, color, transition), 3);
            Primitive.DrawLineSegment(spriteBatch, p4, p1, Color.Lerp(Color.Black, color, transition), 3);
        }

        public bool isOutOfScreen()
        {
            return new Rectangle(-100, -100, (int)Game1.size.X + 200, (int)Game1.size.Y + 200).Contains((int)p1.X, (int)p1.Y) == false
                && new Rectangle(-100, -100, (int)Game1.size.X + 200, (int)Game1.size.Y + 200).Contains((int)p2.X, (int)p2.Y) == false
                && new Rectangle(-100, -100, (int)Game1.size.X + 200, (int)Game1.size.Y + 200).Contains((int)p3.X, (int)p3.Y) == false
                && new Rectangle(-100, -100, (int)Game1.size.X + 200, (int)Game1.size.Y + 200).Contains((int)p4.X, (int)p4.Y) == false;
        }

        #region special
        public void SpecialUpdate()
        {
            if (specialDirection == Direction.left && move)
                MoveLeft();
            else if (specialDirection == Direction.right && move)
                MoveRight();
            else if (specialDirection == Direction.down && move) 
                MoveDown();
        }

        public void MoveLeft()
        {
            SpecialStart();
            if (moveLeft)
            {
                specialSpeed += specialAcc;
                position.X -= specialSpeed;
            }
                
            else
            {
                //specialSpeed += specialAcc;
                position.X += specialSpeed;
            }

            if (position.X >= Game1.size.X / 2 - 37)
            {
                moveLeft = true;
                SpecialReset();
                position.X = Game1.size.X / 2 - 37;
            }

            if (position.X < 100)
            {
                moveLeft = false;
            }
                
        }

        public void MoveRight()
        {
            SpecialStart();
            if (moveRight)
            {
                specialSpeed += specialAcc;
                position.X += specialSpeed;
            }
            else
            {
                //specialSpeed += specialAcc;
                position.X -= specialSpeed;
            }

            if (position.X <= Game1.size.X / 2 + 37)
            {
                moveRight = true;
                SpecialReset();
                position.X = Game1.size.X / 2 + 37;
            }

            if (position.X > Game1.size.X - 100)
            {
                moveRight = false;
            }
        }

        public void MoveDown()
        {
            SpecialStart();
            if (moveDown)
            {
                specialSpeed += specialAcc;
                position.Y += specialSpeed;
            }
            else
            {
                //specialSpeed += specialAcc;
                position.Y -= specialSpeed;
            }

            if (position.Y <= Game1.size.Y / 2)
            {
                moveDown = true;
                SpecialReset();
                position.Y = Game1.size.Y / 2;
            }

            if (position.Y > Game1.size.Y - 30)
            {
                moveDown = false;
            }
        }

        public void Rotate()
        {
            angle += 0.3f;
        }

        public void SpecialStart()
        {
            Rotate();
            color = Color.MediumVioletRed;
        }

        public void SpecialReset()
        {
            move = false;
            angle = MathHelper.ToRadians(45);
            specialAcc += 0.01f;
            color = Color.White;
        }

        #endregion

        public void FollowLine()
        {
            if (specialDirection == Direction.left)
                position.Y = V.Instance.point1.Y + (position.X - V.Instance.point1.X) * (V.Instance.point1.Y - V.Instance.point2.Y) / (V.Instance.point1.X - V.Instance.point2.X);
            else
                position.Y = V.Instance.point4.Y - (position.X - V.Instance.point4.X) * -(V.Instance.point4.Y - V.Instance.point5.Y) / (V.Instance.point4.X - V.Instance.point5.X);

            angle = (float)Math.Atan2(velocity.X, -velocity.Y);
        }

    }
}
