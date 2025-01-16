namespace ThreeInARow.Core;

public class ElementProducer(Dictionary<int, List<int>> elements)
{
    public int CurrentFor(int column)
    {
        return elements[column].First();
    }

    public void MoveNext(int column)
    {
        elements[column].RemoveAt(0);
    }

    public bool HasNext(int column)
    {
        return elements[column].Any();
    }
}