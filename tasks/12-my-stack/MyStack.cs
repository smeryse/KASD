using Task10.Collection;

namespace Task12.Collection
{
    public class MyStack<T> : MyVector<T>
    {
        private int Size => Size();

        public T this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public void Push(T item)
        {
            Add(item);
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            int topIndex = Size - 1;
            T item = this[topIndex];
            RemoveAt(topIndex);

            return item;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            return this[Size - 1];
        }

        public bool Empty()
        {
            return IsEmpty();
        }

        public int Search(T item)
        {
            int index = LastIndexOf(item);
            if (index == -1)
                return -1;

            return Size - index;
        }
    }
}
