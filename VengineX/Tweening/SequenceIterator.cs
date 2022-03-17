using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Tweening
{
    public static class SequenceIterator
    {
        public static IEnumerator<(int, bool)> GetIteratorFor(Direction direction, int length)
        {
            return direction switch
            {
                Direction.Normal => Normal(length),
                Direction.Reverse => Reverse(length),
                Direction.Alternate => Alternate(length),
                Direction.AlternateReverse => AlternateReverse(length),
                _ => throw new NotImplementedException(),
            };
        }


        private static IEnumerator<(int, bool)> Normal(int length)
        {
            while (true)
            {
                for (int i = 0; i < length; i++)
                {
                    yield return (i, false);
                }
            }
        }


        private static IEnumerator<(int, bool)> Reverse(int length)
        {
            while (true)
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    yield return (i, false);
                }
            }
        }


        private static IEnumerator<(int, bool)> Alternate(int length)
        {
            int supressToggle = length / 2 + 1;

            while (true)
            {
                if (length == 1)
                {
                    yield return (0, true);
                }
                else
                {
                    int index = 0;

                    for (int i = 0; i < length * 2 - 2; i++)
                    {
                        if (supressToggle >= 0)
                        {
                            yield return (index, false);
                            supressToggle--;
                        }
                        else
                        {
                            yield return (index, (i + 2) % 3 == 0);
                        }

                        if (i <= length / 2)
                        {
                            index++;
                        }
                        else
                        {
                            index--;
                        }
                    }
                }
            }
        }


        private static IEnumerator<(int, bool)> AlternateReverse(int length)
        {
            while (true)
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    if (i == length - 1)
                    {
                        yield return (i, true);
                    }
                    else
                    {
                        yield return (i, false);
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    if (i == 0)
                    {
                        yield return (i, true);
                    }
                    else
                    {
                        yield return (i, false);
                    }
                }
            }
        }
    }
}
