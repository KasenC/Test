using System;
using SFML.System;
using vector2f = SFML.System.Vector2f;
using System.Threading.Tasks;


namespace EllixEngine
{
    class Physics
    {
        bool independentTime = true;
        long currentTime;
        Clock clock;
        Time time1;
        Time time2;

        public Physics() {
            clock = new Clock();
            time1 = clock.ElapsedTime;
        }

        public Collision collisionDetection(GameObject obj1, GameObject obj2)
        {
            float t1 = obj1.position.Y - obj1.colliderHeight;
            float b1 = obj1.position.Y + obj1.colliderHeight; 
            float l1 = obj1.position.X - obj1.colliderWidth;
            float r1 = obj1.position.X + obj1.colliderWidth;

            float t2 = obj2.position.Y - obj2.colliderHeight;
            float b2 = obj2.position.Y + obj2.colliderHeight;
            float l2 = obj2.position.X - obj2.colliderWidth;
            float r2 = obj2.position.X + obj2.colliderWidth;

            Collision collision = new Collision();
            int vSide = 0, hSide = 0, collisionPlane = 0; //collisionplane 0 = undefined, 1 = vertical, -1 = horizontal
            float width, height;                          //hSide 0 = undefined, 1 = left, -1 = right
                                                          //vSide 0 = undefined, 1 = top, -1 = bottom
            collision.rectTop = max(t1, t2);
            collision.rectBottom = min(b1, b2);
            collision.rectLeft = max(l1, l2);
            collision.rectRight = min(r1, r2);

            width = collision.rectWidth();
            height = collision.rectHeight();

            if (width <= 0 || height <= 0)
                return null;

            collision.obj = obj2;

            if(collision.rectTop == t1 && collision.rectBottom != b1)
                vSide = 1;
            else if(collision.rectBottom == b1 && collision.rectTop != t1)
                vSide = -1;
            else
            {
                Console.WriteLine("Full overlap vertical");
                collisionPlane += 1;
            }

            if(collision.rectLeft == l1 && collision.rectRight != r1)
                hSide = 1;
            else if(collision.rectRight == r1 && collision.rectLeft != l1)
                hSide = -1;
            else
            {
                Console.WriteLine("Full overlap horizontal");
                collisionPlane -= 1;
            }

            if (collisionPlane == 0)
                if (width > height && vSide != 0)
                    collisionPlane = -1;
                else if (height > width && hSide != 0)
                    collisionPlane = 1;
                else if(height == width)
                {
                    if(obj1.velocity.X > obj1.velocity.Y)
                    {
                        collisionPlane = 1;
                    }
                    else if(obj1.velocity.Y >= obj1.velocity.X)
                    {
                        collisionPlane = -1;
                    }
                    else{
                        Console.WriteLine("Undefined collision occurred: ");
                        Console.WriteLine(obj1);
                        Console.WriteLine(obj2);
                        return null;
                    }
                }
                
            if(collisionPlane == 1)
            {
                collision.verticalPlane = true;
                collision.horizontalPlane = false;
                collision.verticalSide = vSide;
                collision.horizontalSide = hSide;
                collision.offset.X = collision.rectWidth() * hSide;
                collision.velocity.X = -obj1.velocity.X;
            }
            else if(collisionPlane == -1)
            {
                collision.verticalPlane = false;
                collision.horizontalPlane = true;
                collision.verticalSide = vSide;
                collision.horizontalSide = hSide;
                collision.offset.Y = collision.rectHeight() * vSide;
                collision.velocity.Y = -obj1.velocity.Y;
            }
            else
            {
                Console.WriteLine("Undefined collision");
            }

            Console.Write("Collision: ");
            Console.Write(collision.rectWidth());
            Console.Write(" x ");
            Console.WriteLine(collision.rectHeight());

            return collision;
        }

        private float max(float f1, float f2)
        {
            if (f1 > f2)
                return f1;
            else
                return f2;
        }

        private float min(float f1, float f2)
        {
            if (f1 < f2)
                return f1;
            else
                return f2;
        }

        private int max(int i1, int i2)
        {
            if (i1 > i2)
                return i1;
            else
                return i2;
        }

        private int min(int i1, int i2)
        {
            if (i1 < i2)
                return i1;
            else
                return i2;
        }

        private long deltaTime() {
            time2 = clock.ElapsedTime;
            long deltaTime = time2.AsMicroseconds() - time1.AsMicroseconds();
            time1 = time2;
            return deltaTime;
        }

        private vector2f calculateDistance(GameObject obj,long deltaT) {
            return (obj.velocity *(float)deltaT/16666);
        }

        /*public calculateAcceleration(GameObject obj,long deltaT)
        {

        }*/

        public void updatePhysics(GameObject[,] objArray, int[] numObj, bool[][] layerInteraction)
        {
            long deltaT = deltaTime();
            Console.WriteLine(deltaT);
            Collision newCollision;
            Collision[] collision = new Collision[100];
            float largestArea = 0;
            int numCollision = 0, largestIndex = 0;    

            for (int i = 0; i < numObj.Length; i++)
            {
                for (int j = 0; j < numObj[i]; j++)
                {
                    if(!objArray[i,j].Fixed)
                    {
                        objArray[i, j].position += calculateDistance(objArray[i, j], deltaT);

                        if (objArray[i, j].hasCollider)
                        {
                            for(int k = 0; k < numObj.Length; k++)
                            {
                                if(layerInteraction[max(i,k)][min(i,k)])
                                {
                                    for(int l = 0; l < numObj[k]; l++)
                                    {
                                        if(objArray[k,l].hasCollider && objArray[k,l] != objArray[i,j])
                                        {
                                            newCollision = collisionDetection(objArray[i, j], objArray[k, l]);
                                            if(newCollision != null)
                                            {
                                                collision[numCollision] = newCollision;
                                                numCollision++;
                                            }
                                        }
                                    }

                                    if (numCollision == 1)
                                    {
                                        objArray[i, j].position += collision[0].offset;
                                        objArray[i, j].velocity += collision[0].velocity;
                                    }
                                    else if(numCollision != 0)
                                    {
                                        for (int n = 0; n < numCollision; n++)
                                        {
                                            if (collision[n].area() > largestArea)
                                            {
                                                largestArea = collision[n].area();
                                                largestIndex = n;
                                            }
                                        }
                                        objArray[i, j].position += collision[largestIndex].offset;
                                        objArray[i, j].velocity += collision[largestIndex].velocity;
                                    }

                                    largestArea = 0;
                                    numCollision = 0;
                                    largestIndex = 0;
                                }
                            }
                        }
                    } 
                }
            }
        }

    }

    class Collision
    {
        public float rectTop, rectBottom, rectLeft, rectRight;
        public bool horizontalPlane = false, verticalPlane = false;
        public int verticalSide = 0, horizontalSide = 0;
        public vector2f offset, velocity;
        public GameObject obj;

        public float rectHeight()
        {
            return rectBottom - rectTop;
        }
        public float rectWidth()
        {
            return rectRight - rectLeft;
        }
        public float area()
        {
            return rectWidth() * rectHeight();
        }
    }
}
