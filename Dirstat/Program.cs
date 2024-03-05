// See https://aka.ms/new-console-template for more information

using DirStat.Core;

Console.WriteLine("Hello, World!");

// Crawling D:\Pics
// Total Number of Files: 106411
// Total Size in Bytes: 390,403,999,923.00 (363.59 GB)

// Crawling E:\Cloud\Google Drive\My Drive\Pics
// Total Number of Files: 106345
// Total Size in Bytes: 390,403,983,865.00 (363.59 GB)


// var crawler = new Crawler(@"D:\Pics");
var crawler = new Crawler(@"E:\Cloud\Google Drive\My Drive");
await crawler.Crawl();