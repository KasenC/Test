using System;
using SFML.Window;

namespace EllixEngine
{
    class mainClass
    {
        //Main
        static void Main(string[] args)
        {
            //Create Game Engine
            Ellix gameEngine = new Ellix();
            gameEngine.Init("Powered By Ellix", 960, 540);

            gameEngine.registerLayers(5, 11000);
            gameEngine.layerCollision(5);

            //Create Objects
            Chunk chunk1 = new Chunk();
            chunk1.create("block.png", 1);
            chunk1.register(gameEngine);

            MyPlayer player = new MyPlayer();
            player.setImage("player.png");
            player.position = new SFML.System.Vector2f(0, -30);
            player.setAnimation(30,90,1,1,1);
            player.setupCollider();
            gameEngine.registerObject(player, 1);

            PlayerCam camera = new PlayerCam(player);
            gameEngine.registerCamera(camera);

            /*
            GameObject obj1 = new GameObject();
            obj1.setImage("C:\\Assets\\Pokemon_Go.png");
            gameEngine.registerObject(obj1,0);
            obj1.scale = new SFML.System.Vector2f(1f, 1f);

            GameObject obj2 = new GameObject();
            obj2.setImage("C:\\Assets\\Pokemon_Go.png");
            obj2.position = new SFML.System.Vector2f(300, 0);
            gameEngine.registerObject(obj2, 0);
            */

            //Create inputs
            gameEngine.registerInput(Keyboard.Key.A, "left");
            gameEngine.registerInput(Keyboard.Key.W, "up");
            gameEngine.registerInput(Keyboard.Key.D, "right");
            gameEngine.registerInput(Keyboard.Key.S, "down");

            //Main loop
            while (gameEngine.checkWindowOpen())
            {
                
                gameEngine.renderFrame();
                gameEngine.getInput();
                gameEngine.applyPhysics();
                gameEngine.update();

                //System.Threading.Thread.Sleep(10);
            }
        }
    }

    class MyPlayer:Player
    {
        public override void update(Input input)
        {
            base.update(input);
            velocity.X = 0;
            velocity.Y = 0;
            int speed = 5;
            if (input.keyDown("left"))
            {
                velocity.X = -speed;
            }

            if (input.keyDown("right"))
            {
                velocity.X = speed;
            }

            if (input.keyDown("up"))
            {
                velocity.Y = -speed;
            }
            if (input.keyDown("down"))
            {
                velocity.Y = speed;
            }
        }
    }
}
