using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace NPBehave
{
    /// <summary>
    /// 并行节点
    /// 并行运行子节点。
    /// failurePolicy为Policy.ONE。当其中一个孩子失败时，并行就会停止，返回失败。
    /// successPolicy为Policy.ONE。当其中一个孩子失败时，并行将停止，返回成功。
    /// 如果并行没有因为Policy.ONE而停止。它会一直执行，直到所有的子节点都完成，然后如果所有的子节点都成功或者失败，它就会返回成功。
    /// </summary>
    public class Parallel : Composite
    {
        public enum Policy
        {
            ONE,
            ALL,
        }

        // private Wait waitForPendingChildrenRule;
        private Policy failurePolicy;
        private Policy successPolicy;
        private int childrenCount = 0;
        private int runningCount = 0;
        private int succeededCount = 0;
        private int failedCount = 0;
        private Dictionary<Node, bool> childrenResults;
        private bool successState;
        private bool childrenAborted;

        public Parallel(Policy successPolicy, Policy failurePolicy, /*Wait waitForPendingChildrenRule,*/ params Node[] children) : base("Parallel", children)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
            // this.waitForPendingChildrenRule = waitForPendingChildrenRule;
            this.childrenCount = children.Length;
            this.childrenResults = new Dictionary<Node, bool>();
        }

        protected override void DoStart()
        {
            foreach (Node child in Children)
            {
                Assert.AreEqual(child.CurrentState, State.INACTIVE);
            }

            childrenAborted = false;
            runningCount = 0;
            succeededCount = 0;
            failedCount = 0;
            foreach (Node child in this.Children)
            {
                runningCount++;
                child.Start();
            }
        }

        protected override void DoStop()
        {
            Assert.IsTrue(runningCount + succeededCount + failedCount == childrenCount);

            foreach (Node child in this.Children)
            {
                if (child.IsActive)
                {
                    child.Stop();
                }
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            runningCount--;
            if (result)
            {
                succeededCount++;
            }
            else
            {
                failedCount++;
            }
            this.childrenResults[child] = result;

            bool allChildrenStarted = runningCount + succeededCount + failedCount == childrenCount;
            if (allChildrenStarted)
            {
                if (runningCount == 0)
                {
                    if (!this.childrenAborted) // if children got aborted because rule was evaluated previously, we don't want to override the successState 
                    {
                        if (failurePolicy == Policy.ONE && failedCount > 0)
                        {
                            successState = false;
                        }
                        else if (successPolicy == Policy.ONE && succeededCount > 0)
                        {
                            successState = true;
                        }
                        else if (successPolicy == Policy.ALL && succeededCount == childrenCount)
                        {
                            successState = true;
                        }
                        else
                        {
                            successState = false;
                        }
                    }
                    Stopped(successState);
                }
                else if (!this.childrenAborted)
                {
                    Assert.IsFalse(succeededCount == childrenCount);
                    Assert.IsFalse(failedCount == childrenCount);

                    if (failurePolicy == Policy.ONE && failedCount > 0/* && waitForPendingChildrenRule != Wait.ON_FAILURE && waitForPendingChildrenRule != Wait.BOTH*/)
                    {
                        successState = false;
                        childrenAborted = true;
                    }
                    else if (successPolicy == Policy.ONE && succeededCount > 0/* && waitForPendingChildrenRule != Wait.ON_SUCCESS && waitForPendingChildrenRule != Wait.BOTH*/)
                    {
                        successState = true;
                        childrenAborted = true;
                    }

                    if (childrenAborted)
                    {
                        foreach (Node currentChild in this.Children)
                        {
                            if (currentChild.IsActive)
                            {
                                currentChild.Stop();
                            }
                        }
                    }
                }
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            if (immediateRestart)
            {
                Assert.IsFalse(abortForChild.IsActive);
                if (childrenResults[abortForChild])
                {
                    succeededCount--;
                }
                else
                {
                    failedCount--;
                }
                runningCount++;
                abortForChild.Start();
            }
            else
            {
                throw new Exception("On Parallel Nodes all children have the same priority, thus the method does nothing if you pass false to 'immediateRestart'!");
            }
        }
    }
}