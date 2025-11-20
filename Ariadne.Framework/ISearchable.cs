namespace Ariadne.Framework
{
    public interface ISearchable
    {
        public void Search(string searchText);

        public IEnumerable<string> Suggestions { get; set; }
    }

    public interface IAriadneFeature
    {

    }
}
