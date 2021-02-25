using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BlockingStack
{
    public class Node<T>
    { public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }
    public class NodeStack<T> : IEnumerable<T>
        {
            Node<T> head;
            int count;
            public static Mutex mtx = new Mutex();
            public bool IsEmpty()
            {
                mtx.WaitOne();
                mtx.ReleaseMutex();
                return count == 0;
            }
            public int Count()
            {
                mtx.WaitOne();
                mtx.ReleaseMutex();
                return count;
            }
            public void Push(T item)
            {
                mtx.WaitOne();
                Node<T> node = new Node<T>(item);
                node.Next = head;
                head = node;
                count++;
                mtx.ReleaseMutex();
            }
            public T Pop()
            {
                mtx.WaitOne();
                if (IsEmpty())
                    throw new InvalidOperationException("Стек пуст");
                Node<T> temp = head;
                head = head.Next;
                count--;
                return temp.Data;
            }
            public T Peek()
            {
                mtx.WaitOne();
                if (IsEmpty())
                    throw new InvalidOperationException("Стек пуст");
                mtx.ReleaseMutex();
                return head.Data;
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this).GetEnumerator();
            }
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                Node<T> current = head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }
}