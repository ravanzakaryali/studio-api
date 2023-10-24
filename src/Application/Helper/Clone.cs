using System.Runtime.Serialization.Formatters.Binary;

namespace Space.Application.Helper;

public static class Clone
{
    public static T DeepCopy<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("Object cannot be null");
        }

        // Create a new BinaryFormatter
        IFormatter formatter = new BinaryFormatter();

        // Create a memory stream to serialize the object
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, obj); // Serialize the object to the stream

            stream.Seek(0, SeekOrigin.Begin); // Reset the stream position

            return (T)formatter.Deserialize(stream); // Deserialize the object from the stream
        }
    }

}
