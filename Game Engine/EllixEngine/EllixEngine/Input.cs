using System;

namespace EllixEngine
{
    class Input
    {
        keyInput[] registeredKeys = new keyInput[84];
        int numKeys = 0;

        public void registerKey(keyInput key)
        {
            registeredKeys[numKeys] = key;
            numKeys++;
        }

        public bool keyDown(string name)
        {
            for(int i = 0; i < numKeys; i++)
            {
                if(registeredKeys[i].name == name)
                {
                    return registeredKeys[i].keyDown();
                }
            }
            return false;
        }

        public bool keyPressed(string name)
        {
            for (int i = 0; i < numKeys; i++)
            {
                if (registeredKeys[i].name == name)
                {
                    return registeredKeys[i].keyPressed();
                }
            }
            return false;
        }

        public bool keyReleased(string name)
        {
            for (int i = 0; i < numKeys; i++)
            {
                if (registeredKeys[i].name == name)
                {
                    return registeredKeys[i].keyReleased();
                }
            }
            return false;
        }
    }
}
