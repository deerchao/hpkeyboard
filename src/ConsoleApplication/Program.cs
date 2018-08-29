using HpKeyService;

namespace ConsoleApplication
{
    class Program
    {
        public static object HPKeyboard { get; private set; }

        static void Main(string[] args)
        {
            HpKeyboard.Run();
        }
    }
}
