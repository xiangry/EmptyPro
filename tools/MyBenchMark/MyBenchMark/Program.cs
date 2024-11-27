// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using MyBenchMark;

Console.WriteLine("Hello, World!");


BenchmarkRunner.Run<MyTest>();