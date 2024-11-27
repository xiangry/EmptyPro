using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Microsoft.CodeAnalysis;
using Platform = BenchmarkDotNet.Environments.Platform;

namespace MyBenchMark;


public class NoOptimizationConfig : ManualConfig
{
    public NoOptimizationConfig()
    {
        AddJob(Job.Default
            .WithJit(Jit.RyuJit)          // 使用旧版 JIT
            .WithPlatform(Platform.X86));     // 强制 32 位架构
            // .WithOptimizationLevel(OptimizationLevel.None)); // 禁用优化
    }
}



// [MemoryDiagnoser]
// [Config(typeof(NoOptimizationConfig))]
public class MyTest
{
    private int[] data = null;

    [GlobalSetup]
    public void Setup()
    {
        data = new int[10000];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i;
        }
    }
    
    [Benchmark]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public int SumWithForLoop()
    {
        var sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }

        return sum;
    }
    
    [Benchmark]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public int SumWithLinq()
    {
        return data.Sum();
    }
}