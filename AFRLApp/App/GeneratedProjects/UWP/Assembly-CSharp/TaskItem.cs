using System;
using System.Linq;
using System.Text;

public class TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    public byte[] Attachment { get; set; }

    public TaskItem()
    {
        IsCompleted = false;
    }

    public TaskItem(int id, string name)
    {
        Id = id;
        Name = Name;
        IsCompleted = false;
    }

    public static TaskItem FromByteArray(byte[] bytes)
    {
        var id = BitConverter.ToInt32(SubArray(bytes, 0, 4), 0);
        var isCompleted = BitConverter.ToBoolean(SubArray(bytes, 4, 1), 0);

        var nameLength = BitConverter.ToInt32(SubArray(bytes, 5, 4), 0);
        var currentPosition = 9 + nameLength;
        var name = Encoding.ASCII.GetString(SubArray(bytes, 9, nameLength));

        var imageLength = BitConverter.ToInt32(SubArray(bytes, currentPosition, 4), 0);
        var imageBytes = SubArray(bytes, currentPosition + 4, imageLength);

        return new TaskItem
        {
            Id = id,
            Name = name,
            IsCompleted = isCompleted,
            Attachment = imageBytes
        };
    }
    public byte[] ToByteArray()
    {
        var idBytes = BitConverter.GetBytes(Id);

        var nameBytes = Encoding.ASCII.GetBytes(Name);
        var nameBytesLength = BitConverter.GetBytes(nameBytes.Length);

        var completedBytes = BitConverter.GetBytes(IsCompleted);
        var imageBytesLength = BitConverter.GetBytes(Attachment.Length);

        var allBytes = idBytes.Concat(completedBytes)
            .Concat(nameBytesLength)
            .Concat(nameBytes)
            .Concat(imageBytesLength)
            .Concat(Attachment)
            .ToArray();

        return (BitConverter.GetBytes(allBytes.Length + 4)).Concat(allBytes).ToArray();
    }

    public static byte[] SubArray(byte[] data, int start, int length)
    {
        byte[] toReturn = new byte[length];
        Array.Copy(data, start, toReturn, 0, length);
        return toReturn;
    }
}