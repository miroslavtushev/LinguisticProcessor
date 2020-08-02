namespace SEEL.LinguisticProcessor
{
    public struct Word
    {
        public string Name { get; set; }
        /// <summary>
        /// S = source code, C = comment
        /// </summary>
        public char Type { get; set; }
        public Word(string n, char t = 'S')
        {
            Name = n.ToLower();
            Type = t;
        }
        public static bool operator ==(Word w1, Word w2)
        {
            return w1.Name == w2.Name && w1.Type == w2.Type;
        }
        public static bool operator !=(Word w1, Word w2)
        {
            return w1.Name != w2.Name || w1.Type != w2.Type;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
