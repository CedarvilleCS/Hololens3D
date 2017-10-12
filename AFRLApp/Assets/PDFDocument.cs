using System;
using System.Collections.Generic;

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
}
