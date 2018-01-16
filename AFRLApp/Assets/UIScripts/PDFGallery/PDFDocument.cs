﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PDFDocument
{
    public int id;
    public List<byte[]> pages;
    public Texture2D thumbnail = new Texture2D(2, 2);
    

	public PDFDocument()
	{
        this.id = 0;
        this.pages = new List<byte[]>();
	}

    public PDFDocument(int id, List<byte[]> pages)
    {
        this.id = id;
        this.pages = pages;
        thumbnail.LoadImage(pages[0]);
    }
    public static PDFDocument FromByteArray(byte[] bytes)
    {   
        //first four bytes is id
        int id = BitConverter.ToInt32(SubArray(bytes, 0, 4), 0);
        //next four bytes is number of pages
        int numPages = BitConverter.ToInt32(SubArray(bytes, 4, 4), 0);
        //setup for getting pages loop
        List<byte[]> pagesToSet = new List<byte[]>();
        int startIndex = 8;
        //this loop adds pages
        for(int i = 0; i < numPages; i++){
            //get page length (will always be 4 bytes)
            int pageLength = BitConverter.ToInt32(SubArray(bytes, startIndex, 4), 0);
            startIndex += 4;
            //get page data using length
            byte[] pageData = SubArray(bytes, startIndex, pageLength);
            pagesToSet.Add(pageData);
            startIndex += pageLength;
        }
        return new PDFDocument(id, pagesToSet);
    }

    public byte[] ToByteArray()
    {
        //start with ID, which is a byte[] of length 4
        byte[] toReturn = BitConverter.GetBytes(id);
        //append number of pages
        toReturn = Concat(toReturn, BitConverter.GetBytes(pages.Count));
        foreach(byte[] page in pages){
            //append 4-byte length to beginning
            toReturn = Concat(toReturn, BitConverter.GetBytes(page.Length));
            //append page
            toReturn = Concat(toReturn, page);
        }
        return toReturn;
    }

    public static byte[] Concat(byte[] arr1, byte[] arr2)
    {
        byte[] toReturn = new byte[arr1.Length + arr2.Length];
        arr1.CopyTo(toReturn, 0);
        arr2.CopyTo(toReturn, arr1.Length);
        return toReturn;
    }

    public static byte[] SubArray(byte[] data, int start, int length){
        byte[] toReturn = new byte[length];
        Array.Copy(data, start, toReturn, 0, length);
        return toReturn;
    }
}