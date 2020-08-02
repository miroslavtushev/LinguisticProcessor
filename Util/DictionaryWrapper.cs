using System.Collections.Generic;

namespace SEEL.LinguisticProcessor.Util
{
    /// <summary>
    /// Used to pass dictionaries with their names for correct output for UI
    /// </summary>
    public class DictionaryWrapper
    {
        public Dictionary<Word, int> Dict { get; private set; }
        public string Name { get; private set; }

        public DictionaryWrapper(Dictionary<Word, int> dict, string name)
        {
            Dict = dict;
            Name = name;
        }
    }
}
