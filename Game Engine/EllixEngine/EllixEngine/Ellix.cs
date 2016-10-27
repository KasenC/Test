using System;
using SFML.Graphics;
using SFML.Window;


namespace EllixEngine
{
    class Ellix
    {
        Renderer gameRender;
        RenderWindow gameWindow;
        Input input = new Input();
        GameObject[,] objArray;
        int[] numObj;
        bool[][] layerInteraction;
        Physics physicsEngine;
           
        
        //Initialization
        public void Init(String windowTitle,uint ResWidth,uint ResHeight)
        {
            gameRender = new Renderer();
            gameWindow = gameRender.init(windowTitle,ResWidth,ResHeight);
            //Input
            gameWindow.Closed += new EventHandler(OnClose);
            gameWindow.KeyPressed += new EventHandler<KeyEventArgs>(KeyPressed);
            gameWindow.Resized += new EventHandler<SizeEventArgs>(Resized);
            physicsEngine = new Physics();
        }

        //Escape Method For Debug Purposes
        void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                gameWindow.Close();
            }
        }

        //Resizing
        public void Resized(object sender,SizeEventArgs resize)
        {
            gameWindow.SetView(new View(((SFML.System.Vector2f) gameWindow.Size) / 2,(SFML.System.Vector2f) gameWindow.Size));
        }

        //X closes window
        void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }


        public void registerInput(Keyboard.Key key, string name, bool requireFocus = true)
        {
            keyInput inputKey = new keyInput(key, gameWindow, name, requireFocus);
            input.registerKey(inputKey);
        }


        public bool checkWindowOpen() {
            return gameWindow.IsOpen;
        }

        //GameEngine update method
        public void update()
        {
            for(int i = 0; i < numObj.Length; i++)
            {
                for (int j = 0; j < numObj[i]; j++) {
                    objArray[i, j].update(input);
                }
            }
        }


        //Rendering
        public void renderFrame()
        {
            gameRender.frame(gameWindow, objArray, numObj);
        }

        //Event Polling
        public void getInput()
        {
            gameWindow.DispatchEvents();
        }

        //setting layer Collision
        public void layerCollision(int numLayers) {
            layerInteraction = new bool[numLayers][];
            for (int i = 0; i < numLayers; i++) {
                layerInteraction[i] = new bool[i+1];
                for (int j = 0; j <= i; j++)
                {
                    layerInteraction[i][j] = true;
                }
            }
        }   

        public void setLayerCollision(int layerA, int layerB, bool shouldCollide) {
            layerInteraction[layerA][layerB] = shouldCollide;
        }

        //Calling physics engine
        public void  applyPhysics() {
            physicsEngine.updatePhysics(objArray,numObj, layerInteraction);
        }

        //Registering Objects
        public void registerObject(GameObject obj, int layer = 0)
        {
            obj.layer = layer;
            obj.arrayPos = numObj[layer];
            objArray[layer, numObj[layer]] = obj;
            numObj[layer]++;
        }

        public void registerLayers(int numLayers, int layerSize)
        {
            numObj = new int[numLayers];
            objArray = new GameObject[numLayers, layerSize];
            layerCollision(numLayers);
        }

        public void registerCamera(Camera cam)
        {
            gameRender.registerCamera(cam);
            registerObject(cam);
        }

        //Deleting GameObject
        public void deleteObject(ref GameObject obj)
        {
            removeFromArray(objArray, ref numObj[obj.layer], obj.layer, obj.arrayPos);
            obj = null;
        }

        private void removeFromArray(GameObject[,] array, ref int numItems, int layer, int itemNumber)
        {
            numItems -= 1;
            array[layer, itemNumber] = array[layer, numItems];
            array[layer, numItems] = null;
            array[layer, itemNumber].arrayPos = itemNumber;
        }
    }
}
