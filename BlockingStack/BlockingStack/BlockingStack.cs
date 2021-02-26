using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BlockingStack
{
    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    public class NodeStack<T> : IEnumerable<T>
    {
        Node<T> _head;
        private int _count;
        private static AutoResetEvent _modificatingOrReading = new AutoResetEvent(true);

        public bool IsEmpty()
        {
            _modificatingOrReading.WaitOne();
            bool empty = (_count == 0);
            _modificatingOrReading.Set();
            return empty;
        }

        public int Count()
        {
            _modificatingOrReading.WaitOne();
            int count = _count;
            _modificatingOrReading.Set();
            return count;
        }

        public void Push(T item)
        {
            _modificatingOrReading.WaitOne();
            Node<T> node = new Node<T>(item);
            node.Next = _head;
            _head = node;
            _count++;
            _modificatingOrReading.Set();
        }

        public T Pop()
        {
            _modificatingOrReading.WaitOne();
            if (_count==0)
                throw new InvalidOperationException("Стек пуст");
            Node<T> temp = _head;
            _head = _head.Next;
            _count--;
            T extract = temp.Data;
            _modificatingOrReading.Set();
            return extract;
        }

        public T Peek()
        {
            _modificatingOrReading.WaitOne();
            if (_count==0)
                throw new InvalidOperationException("Стек пуст");
            T headdata = _head.Data;
            _modificatingOrReading.Set();
            return headdata;

        }
        public bool Contains(T value, out Node<T> element)
        {
            element = null;
            Node<T> next;

            _modificatingOrReading.WaitOne();
            if (_count != 0)
                next = _head.Next;
            else
            {
                _modificatingOrReading.Set();
                return false;
            }

            while (next != null)
            {
                if (value.Equals(next.Data))
                {
                    element = next;
                    _modificatingOrReading.Set();
                    return true;
                }
                next = next.Next;
            }
            _modificatingOrReading.Set();
            return false;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
    class BlockingStack
    {
        public static void Main()
        {
        }
    }
}