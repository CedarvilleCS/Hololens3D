using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PDFDocument
{
    public int id;
    public List<byte[]> pages;

	public PDFDocument()
	{
        this.id = 0;
        this.pages = new List<byte[]>();
	}

    public PDFDocument(int id, List<byte[]> pages)
    {
        this.id = id;
        this.pages = pages;
    }

    public static PDFDocument FromByteArray(byte[] bytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            PDFDocument obj = (PDFDocument) binForm.Deserialize(memStream);
            return obj;
        }
    }

    public byte[] ToByteArray()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            return ms.ToArray();
        }
    }
}