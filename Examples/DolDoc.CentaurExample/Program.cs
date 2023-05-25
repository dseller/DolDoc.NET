using DolDoc.Centaur;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DolDoc.CentaurExample
{
    public class Program
    {
        public static void Print(string text) => Console.Write(text);

        public static string ToBinary(long value) => Convert.ToString(value, 2);

        public static void LogInfo(string text) => Log.Logger.Information(text);

        public static string Str(object obj) => obj.ToString(); //obj?.ToString() ?? "null";
        
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .MinimumLevel.Debug()
                .CreateLogger();

            var engine = new CentaurEngine(new SeriLogger(Log.Logger));
            engine.LoadFunctions<Program>();
            engine.Include("test", File.ReadAllText("test.centaur"));
            
            var result = engine.Call<object>("my_function");
            Log.Logger.Information("Result: {result}", result);

            // string str;
            // while (!string.IsNullOrWhiteSpace(str = Console.ReadLine()))
            // {
            //     engine.Include("input_" + Guid.NewGuid().ToString("n"), str);
            // }
                
        }
    }
}