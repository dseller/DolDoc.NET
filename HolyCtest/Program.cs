using Antlr4.Runtime;
using DolDoc.HolyC.Grammar;
using HolyScript.Compiler;
using System;
using System.IO;
using Wasmtime;

namespace HolyCtest
{
    internal class Program
    {
        public static void Run(string str)
        {
            var inputStream = new AntlrInputStream(str);
            var lexer = new HolyCLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new HolyCParser(commonTokenStream);

            var visitor = new HolyCVisitor(parser);


            var context = parser.compilationUnit();
            visitor.Visit(context);
            visitor.Finalize();

            // custom sections? stack?

            // Console.WriteLine(visitor.Module..ToString());

            foreach (var c in visitor.Module.Codes)
            {
                foreach (var i in c.Code)
                {
                    Console.WriteLine(i);
                }
            }

            using (var ms = new MemoryStream())
            {
                visitor.Module.WriteToBinary(ms);

                ms.Seek(0, SeekOrigin.Begin);
                var bytes = ms.ToArray();
                
                using (var engine = new Engine())
                {
                    using (var module = Module.FromStream(engine, "test", ms))
                    {

                        using (var linker = new Linker(engine))
                        {
                            using (var store = new Store(engine))
                            {
                                var memory = new Memory(store, 100);
                                linker.Define("io", "log", Function.FromCallback(store, (Caller caller, int addr) =>
                                {
                                    var logString = memory.ReadNullTerminatedString(store, addr);
                                    Console.WriteLine("📜 " + logString);
                                }));

                                linker.Define("io", "assert", Function.FromCallback(store, (Caller caller, int addr1, int success) =>
                                {
                                    var expression = memory.ReadNullTerminatedString(store, addr1);

                                    if (success == 1)
                                    {
                                        // Passed :-)
                                        Console.WriteLine("✔️ Assertion succeeded: " + expression);
                                    }
                                    else
                                    {
                                        throw new Exception("Assertion failed: " + expression);
                                    }
                                }));

                                linker.Define("io", "logInt", Function.FromCallback(store, (Caller caller, int value) =>
                                {
                                    Console.WriteLine("📜 " + value);
                                }));

                                linker.Define("core", "mem", memory);
                                var instance = linker.Instantiate(store, module);
                                var run = instance.GetFunction(store, "___main");
                                run?.Invoke(store);
                            }
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var input = @"
                #define DENNIS 28
                Print(""Running HolyC test code"");
        
                I32 global_var = 42, another_var = 1337;
                U8 my_arr[128];
                U8 my_flag;

                Assert(global_var == 42);
                Assert(another_var == 1337);
                Assert(DENNIS == 28);

                I32 Multiply(I32 value, I32 multiplier)
                {
                    if (multiplier == 0)
                        return DENNIS;
                    else
                    {
                        I32 inner = multiplier;
                        return value * inner;
                    }
                }

                I32 Test()
                {
                    ""Returning 111"";
                    return 111;
                }

                U0 Loop()
                {
                    I32 counter;// = 0;
                    counter = 0;
                    while (counter < DENNIS) {
                        Print(""Hehe"");
                        counter++;
                    }
                }

                I32 *ptr = 500;
                *ptr = 11223344;
                LogInt(*ptr);
                Assert(*ptr == 11223344);
                Assert(ptr == 500);

                Assert(Test == 111);
                Assert(Multiply(4, 8) == 32);
                Loop;
            ";

            var ptrTest = @"
            I32 *ptr = 500;
            *ptr = 11223344;
            LogInt(*ptr);
            LogInt(ptr);
            ";

            Run(ptrTest);
        }
    }
}
