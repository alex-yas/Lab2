using NUnit.Framework;
using BlockingStack;
using System;
using System.Threading;

namespace NUnitTests
{
    public class Tests
    {
        public NodeStack<int> GenerateStack(int min, int max) 
        {
            NodeStack<int> stack = new NodeStack<int>();

            Random rnd = new Random();
            int elementsNum = rnd.Next(min, max);
            for (int i = 0; i < elementsNum; i++)
            {
                int j = rnd.Next(1,1000);
                stack.Push(j);
            }
            return stack;
        }

        [Test]
        public void CountAndGetEnumeratorFunction_TwoThreadsCountingStackSize_BothSizesAreEqual()
        {
            NodeStack<int> stack = GenerateStack(1,100);

            int i = 1,j = 0;
            Thread t1 = new Thread(delegate () { i = stack.Count(); });
            Thread t2 = new Thread(delegate ()
            {
                foreach (var item in stack)
                {
                    j++;
                }
            });
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.AreEqual(i, j);
        }

        [Test]
        public void PushFunction_OneThreadAddsElementAnotherCountsElements_StackSizeEqualsOne()
        {
            NodeStack<int> stack = GenerateStack(4, 4);

            int i = 0;
            Thread t1 = new Thread(delegate () { stack.Push(10001); });
            Thread t2 = new Thread(delegate () { i = stack.Count(); });
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.AreEqual(5, i);
        }

        [Test]
        public void PeekFunction_OneThreadPushesElementAnotherChecksIt_StackSizeEqualsOne()
        {
            NodeStack<int> stack = GenerateStack(1, 1);

            int peek=10001;
            Thread t1 = new Thread( ()=> {
                peek = stack.Peek();
            });
            Thread t2 = new Thread(delegate () { stack.Push(10001);});
            t2.Start();
            t1.Start();
            
            t1.Join();
            t2.Join();
            Assert.AreEqual(peek,stack.Peek());
        }

       [Test]
        public void IsEmptyFunction_OneThreadPushAnotherPop_StackIsEmpty()
        {
            NodeStack<int> stack = new NodeStack<int>();
            
            Thread t1 = new Thread(()=> { stack.Push(10001); });
            Thread t2 = new Thread(delegate () { stack.Pop(); });
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.True( stack.IsEmpty());
        }
        [Test]
        public void ContainsFunction_OneThreadChecksIsStackIsEmpty_StackIsEmpty()
        {
            NodeStack<int> stack = new NodeStack<int>();
            
            Thread t1 = new Thread(() =>
            {
                stack.Push(10001);
                stack.Push(1);
                stack.Push(2);
            });
            Thread t2 = new Thread(delegate () { stack.Pop(); });
            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
            Node<int> element;
            Assert.False( stack.Contains(2,out element));
        }
    }
}