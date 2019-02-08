using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ElevatorCompiler
{
    public abstract class AbstractPlayer
    {
        public abstract void CleanUp();

        internal float scheduledTime;

        internal void EditorUpdateTick()
        {
            if (EditorApplication.timeSinceStartup >= scheduledTime)
                CleanUp();
        }
    }

}