using System;
using System.Collections;
using System.Collections.Generic;

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
            Node<T> head;
            int count;
 
            public bool IsEmpty
            {
                get { return count == 0; }
            }
            public int Count
            {
                get { return count; }
            }
 
            public void Push(T item)
            {
                // увеличиваем стек
                Node<T> node = new Node<T>(item);
                node.Next = head; // переустанавливаем верхушку стека на новый элемент
                head = node;
                count++;
            }
            public T Pop()
            {
                // если стек пуст, выбрасываем исключение
                if (IsEmpty)
                    throw new InvalidOperationException("Стек пуст");
                Node<T> temp = head;
                head = head.Next; // переустанавливаем верхушку стека на следующий элемент
                count--;
                return temp.Data;
            }
            public T Peek()
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Стек пуст");
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
    class Program
    {
        static void Main(string[] args)
        {
            NodeStack<string> stack = new NodeStack<string>();
            stack.Push("Tom");
            stack.Push("Alice");
            stack.Push("Bob");
            stack.Push("Kate");
 
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            string header = stack.Peek();
            Console.WriteLine($"Верхушка стека: {header}");
            Console.WriteLine();
 
            header = stack.Pop();
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }
        }
    }
}