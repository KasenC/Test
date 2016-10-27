using SFML.Window;
using SFML.Graphics;


namespace EllixEngine
{

    class keyInput
    {
        bool keyHeld = false;
        bool initialPress = false;
        bool keyRelease = false;
        Keyboard.Key key;
        public bool requireFocused;
        bool currentlyFocused = true;
        RenderWindow gameWindow;
        public string name;

        public keyInput(Keyboard.Key key,RenderWindow gameWindow, string name, bool requireFocused) {
            this.requireFocused = requireFocused;
            this.gameWindow = gameWindow;
            this.key = key;
            this.name = name;
        }

        private void update()
        {
            currentlyFocused = gameWindow.HasFocus();
                if (Keyboard.IsKeyPressed(key))
                {
                    if (!keyHeld)
                    {
                        initialPress = true;
                    }

                    else
                    {
                        initialPress = false;
                    }

                    keyHeld = true;
                    keyRelease = false;
                }
                else
                {
                    if (keyHeld)
                    {
                        keyRelease = true;
                    }
                    else
                    {
                        keyRelease = false;
                    }
                    initialPress = false;
                    keyHeld = false;
                }
        }

        public bool keyPressed()
        {
            update();
            if (requireFocused && !currentlyFocused)
                return false;
            else
                return initialPress;
        }

        public bool keyReleased() {
            update();
            if (requireFocused && !currentlyFocused)
                return false;
            else
                return keyRelease;
        }

        public bool keyDown() {
            update();
            if (requireFocused && !currentlyFocused)
                return false;
            else
                return keyHeld;
        }
    }
}
