// See https://aka.ms/new-console-template for more information

var matrix = new string[3][];

for (int i = 0; i < 3; i++)
{
    matrix[i] = new string[3];
    matrix[i][0] = "🎁";
    matrix[i][1] = "\ud83c\udfb5";
    matrix[i][2] = "\ud83d\udc14";
}

Console.OutputEncoding = System.Text.Encoding.UTF32;

for(int i = 0; i < 3; i++)
{
    for(int j = 0; j < 3; j++)
    {
        Console.Write(matrix[i][j]);
    }
    Console.WriteLine();
}