using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace NPBehave
{
    /// <summary>
    /// 选择节点
    /// 按顺序运行子节点，直到其中一个子节点成功(如果其中一个子节点成功，则成功)。
    /// </summary>
    public class Selector : Composite
    {
        private int currentIndex = -1;

        public Selector(params Node[] children) : base("Selector", children)
        {
        }


        protected override void DoStart()
        {
            foreach (Node child in Children)
            {
                Assert.AreEqual(child.CurrentState, State.INACTIVE);
            }

            currentIndex = -1;

            ProcessChildren();
        }

        protected override void DoStop()
        {
            Children[currentIndex].Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (result)
            {
                Stopped(true);
            }
            else
            {
                ProcessChildren();
            }
        }

        private void ProcessChildren()
        {
            if (++currentIndex < Children.Length)
            {
                if (IsStopRequested)
                {
                    Stopped(false);
                }
                else
                {
                    Children[currentIndex].Start();
                }
            }
            else
            {
                Stopped(false);
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            int indexForChild = 0;
            bool found = false;
            foreach (Node currentChild in Children)
            {
                if (currentChild == abortForChild)
                {
                    found = true;
                }
                else if (!found)
                {
                    indexForChild++;
                }
                else if (found && currentChild.IsActive)
                {
                    if (immediateRestart)
                    {
                        currentIndex = indexForChild - 1;
                    }
                    else
                    {
                        currentIndex = Children.Length;
                    }
                    currentChild.Stop();
                    break;
                }
            }
        }

        override public string ToString()
        {
            return base.ToString() + "[" + this.currentIndex + "]";
        }
    }
}