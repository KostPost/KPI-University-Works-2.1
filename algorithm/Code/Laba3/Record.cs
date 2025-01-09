namespace Laba3;

public class Record
{
    public int Key { get; set; }
    public string Data { get; set; }

    public Record(int key, string data)
    {
        Key = key;
        Data = data;
    }
}