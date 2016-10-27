using System;
using SFML.Graphics;
using SFML.Window;
using Vector2f = SFML.System.Vector2f;
using Vector2u = SFML.System.Vector2u;

namespace EllixEngine
{
    class Renderer
    {
        Vector2f referenceResolution = new Vector2f(1920, 1080);
        Vector2f resolutionScale;
        Color clearColor = Color.White;
        GameObject camera;
        Vector2u screenSize;

        //Initialization
        public RenderWindow init(String windowTitle, uint ResWidth,uint ResHeight)
        {
            RenderWindow window = new RenderWindow(new VideoMode(ResWidth,ResHeight), windowTitle, Styles.Default);
            return window;
        }

        public void setClearColor(Color newColor) {
            clearColor = newColor;
        }

        public void registerCamera(Camera cam)
        {
            camera = cam;
        }

        //Vector math
        private Vector2f multiplyVector(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }

        private Vector2f divideVector(Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1.X / vector2.X, vector1.Y / vector2.Y);
        }
        
        //Function to check window dimension (called every frame)
        public void setWindowDimension(RenderWindow window)
        {
            screenSize = window.Size;
            resolutionScale = divideVector((Vector2f)screenSize, referenceResolution);
        }

        //Rendering function
        public void frame(RenderWindow window, GameObject[,] objArray, int[] numObj)
        {
            window.Clear(clearColor);
            setWindowDimension(window);
            Vector2f halfSize = (Vector2f) screenSize / 2;
            Vector2f halfSpriteSize;
            for(int i = 0; i < numObj.Length; i++)
            {
                for (int j = 0; j < numObj[i]; j++)
                {
                    if (objArray[i,j].Visible && objArray[i,j].ImageExists)
                    {
                        Sprite sprite = new Sprite();
                        sprite.Texture = new Texture(objArray[i,j].Img);
                        sprite.Scale = multiplyVector(objArray[i,j].scale, resolutionScale);
                        halfSpriteSize = multiplyVector((Vector2f)sprite.Texture.Size / 2, objArray[i,j].scale);
                        sprite.Position = multiplyVector(objArray[i,j].position - camera.position - halfSpriteSize, resolutionScale) + halfSize;
                        window.Draw(sprite);
                        sprite.Texture.Dispose();
                        sprite.Dispose();
                    }
                }
            }
            window.Display();
        }
    }
}
