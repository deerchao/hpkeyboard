using kchordr;
using System.Collections.Generic;
using System.Threading;

namespace HpKeyService
{
    class HpKeyboard
    {
        public static void Run()
        {
            var input = new List<Interception.KeyStroke>(6);

            int device;
            var stroke = new Interception.Stroke();
            var context = Interception.CreateContext();

            Interception.SetFilter(context, Interception.IsKeyboard, Interception.Filter.All);

            try
            {
                while (Interception.Receive(context, device = Interception.Wait(context), ref stroke, 1) > 0)
                {
                    input.Add(stroke.key);

                    while (input.Count > 0)
                    {
                        var match = FindMatch(input);
                        if (match == null)
                        {
                            stroke.key = input[0];
                            Interception.Send(context, device, ref stroke, 1);
                            input.RemoveAt(0);
                            continue;
                        }
                        else
                        {
                            if (match.Length > 0)
                            {
                                for (var i = 0; i < match.Length; i++)
                                {
                                    stroke.key = match[i];
                                    Interception.Send(context, device, ref stroke, 1);
                                }
                                input.Clear();
                            }
                            break;
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                Interception.DestroyContext(context);
            }
        }

        static Interception.KeyStroke[] _insertGuesture = new[]
        {
            new Interception.KeyStroke{ code = 29, state = 0 },
            new Interception.KeyStroke{ code = 56, state = 0 },
            new Interception.KeyStroke{ code = 109, state = 0 },
            new Interception.KeyStroke{ code = 29, state = 1 },
            new Interception.KeyStroke{ code = 56, state = 1 },
            new Interception.KeyStroke{ code = 109, state = 1 },
        };

        static Interception.KeyStroke[] _homeGuesture = new[]
        {
            new Interception.KeyStroke{ code = 56, state = 0 },
            new Interception.KeyStroke{ code = 102, state = 0 },
            new Interception.KeyStroke{ code = 91, state = 2 },
            new Interception.KeyStroke{ code = 56, state = 1 },
            new Interception.KeyStroke{ code = 102, state = 1 },
            new Interception.KeyStroke{ code = 91, state = 3 },
        };

        static Interception.KeyStroke[] _endGuesture = new[]
        {
            new Interception.KeyStroke{ code = 29, state = 0 },
            new Interception.KeyStroke{ code = 56, state = 0 },
            new Interception.KeyStroke{ code = 101, state = 0 },
            new Interception.KeyStroke{ code = 29, state = 1 },
            new Interception.KeyStroke{ code = 56, state = 1 },
            new Interception.KeyStroke{ code = 101, state = 1 },
        };

        static Interception.KeyStroke[] _insert = new[]
        {
            new Interception.KeyStroke{ code = 82, state = 2 },
            new Interception.KeyStroke{ code = 82, state = 3 },
        };

        static Interception.KeyStroke[] _home = new[]
        {
            new Interception.KeyStroke{ code = 71, state = 2 },
            new Interception.KeyStroke{ code = 71, state = 3 },
        };

        static Interception.KeyStroke[] _end = new[]
        {
            new Interception.KeyStroke{ code = 79, state = 2 },
            new Interception.KeyStroke{ code = 79, state = 3 },
        };

        static Interception.KeyStroke[] _empty = new Interception.KeyStroke[0];


        static Interception.KeyStroke[] FindMatch(List<Interception.KeyStroke> input)
        {
            var insert = IsMatch(_insertGuesture, input);
            if (insert == true)
                return _insert;

            var home = IsMatch(_homeGuesture, input);
            if (home == true)
                return _home;

            var end = IsMatch(_endGuesture, input);
            if (end == true)
                return _end;

            if (insert == false && home == false && end == false)
                return null;

            return _empty;
        }

        static bool? IsMatch(Interception.KeyStroke[] guestures, List<Interception.KeyStroke> input)
        {
            if (input.Count > guestures.Length)
                return false;

            for (var i = 0; i < input.Count; i++)
            {
                if (!Equals(guestures[i], input[i]))
                    return false;
            }

            return input.Count == guestures.Length
                ? true as bool?
                : null;
        }

        static bool Equals(Interception.KeyStroke a, Interception.KeyStroke b)
        {
            return a.code == b.code && a.state == b.state;
        }
    }
}
