using System;
using Firefly.Box;
using ENV;

namespace $rootnamespace$
{
    public abstract class $itemname$ : UIControllerBase //TODO: change to BusinessProcessBase if needed
    {
        public static $itemname$ Create()
        {
            return ENV.AbstractFactory.CreateInstance<$itemname$>();
        }

        //TODO: change Run overloads as needed
        public abstract void Run();

    }
}