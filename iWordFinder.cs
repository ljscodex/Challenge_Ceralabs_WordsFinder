namespace Challenge
{
    public interface iWordsFinder
    {
        public IEnumerable<string> Find(IEnumerable<string> wordstream);

    }
}