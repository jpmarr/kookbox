namespace kookbox.core.Interfaces
{
    public class Paging
    {
        public Paging(int start, int count)
        {
            Start = start;
            Count = count;
        }

        private int Start { get; }
        private int Count { get; }
    }
}
