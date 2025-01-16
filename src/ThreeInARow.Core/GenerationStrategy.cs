namespace ThreeInARow.Core;

public interface GenerationStrategy
{
    public bool CanActivate(ElementView[,] elements);

    public bool CanGenerate(ElementView[,] elements);

    public string[] Generate(string[,] elements);
}