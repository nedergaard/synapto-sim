using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Creature.Brain
{
   public static class StringExtensions
    {
        /// <summary>
        /// Return strings of the given length, or the remaining string if shorter. 
        /// </summary>
        /// <param name="s">String to return in chunks</param>
        /// <param name="length">Length of each chunk</param>
        public static IEnumerable<string> Chunks(this IEnumerable<char> s, int length)
        {
            var index = 0;
            while (true)
            {
                var result = string.Join(string.Empty, s.Skip(index).Take(length));
                if (result == string.Empty)
                {
                    yield break;
                }
                index += length;
                yield return result;
            };
        }
    }
}