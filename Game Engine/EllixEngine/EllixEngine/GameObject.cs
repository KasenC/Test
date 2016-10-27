using System;
using SFML.Graphics;

namespace EllixEngine
{
    class GameObject
    {

        public SFML.System.Vector2f position = new SFML.System.Vector2f(0, 0);
        public SFML.System.Vector2f scale = new SFML.System.Vector2f(1, 1);
        public Image Img { get; protected set; }
        public bool ImageExists { get; private set; }
        public bool Visible = true, Fixed = true, CompositeRender = false;
        public int layer = 0, arrayPos = 0;

        //Physics
        public bool hasCollider = false;
        public bool applyGravity = false;
        public SFML.System.Vector2f velocity = new SFML.System.Vector2f(0, 0);
        public SFML.System.Vector2f acceleration = new SFML.System.Vector2f(0, 0);
        public float colliderWidth = 0, colliderHeight = 0;


        public void setImage(string path)
        {
            Img = new Image(path);
            ImageExists = true;
        }

        public void setImage(Image image)
        {
            Img = image;
            ImageExists = true;
        }

        public void setScale(float x, float y)
        {
            scale.X = x;
            scale.Y = y;
        }

        public void setupCollider()
        {
            if(ImageExists)
            {
                hasCollider = true;
                colliderWidth = Img.Size.X * scale.X / 2;
                colliderHeight = Img.Size.Y * scale.Y / 2;
            }
        }

        virtual public void update(Input input)
        {

        }
    }

    class AnimatedGameObject : GameObject {
        Image[] frames;
        public bool shouldAnimate = true;
        int cyclesPerFrame, numFrames;
        int cycleCounter = 0, frameCounter = 0;

        public void setAnimation(int width,int height,int numRows,int numColumns,int numFrames, int cycleCount = 1)
        {
            int x = 0;
            int y = 0;
            int counter = 0;
            this.numFrames = numFrames;
            cyclesPerFrame = cycleCount;
            frames = new Image[numFrames];
            // i is vertical & j is horizontal
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numColumns; j++) {
                    x = width * j;
                    IntRect rect = new IntRect(x, y, width, height);
                    frames[counter] = new Image((uint) width,(uint) height);
                    frames[counter].Copy(Img, 0, 0, rect);
                    counter += 1;
                }
                y += height;
            }
            Img = frames[0];
        }

        public void setFrame(int frameNum)
        {
            Img = frames[frameNum];
        }

        virtual public void animate()
        {
            if(cycleCounter >= cyclesPerFrame || cycleCounter == 0)
            {
                cycleCounter = 0;
                setFrame(frameCounter);
                frameCounter++;
                if(frameCounter >= numFrames)
                {
                    frameCounter = 0;
                }
            }
            cycleCounter++;
        }

        public override void update(Input input)
        {
            base.update(input);
            animate();
        }
    }
    
    class Player:AnimatedGameObject
    {
        public Player()
        {
            Fixed = false;
        }
    }

    class Camera:GameObject
    {
        public Camera()
        {
            Visible = false;
            Fixed = false;
        }
    }

    class PlayerCam:Camera
    {
        Player p1;

        public PlayerCam(Player player)
        {
            Visible = false;
            Fixed = false;
            p1 = player;
        }

        override public void update(Input input)
        {
            base.update(input);
            position = p1.position;
        }
    }
}
