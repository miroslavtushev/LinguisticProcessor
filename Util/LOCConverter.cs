using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEEL.LinguisticProcessor.Util
{
    public class LOCConverter
    {
        public object Input { get; set; }
        private ulong m_number;

        private string[] ending = { "h", "k", "M", "G", "T" };

        public LOCConverter(object input)
        {
            Input = input;
        }

        /// <summary>
        /// Converts input to ulong for further processing
        /// </summary>
        /// <param name="input">Input number (hopefully a NUMBER)</param>
        /// <returns></returns>
        private ulong ConvertToUlong (object input)
        {
            ulong ret;
            try
            {
                ret = (ulong)input;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($@"There was a problem converting to ulong: {e}");
                ret = 0;
            }
            return (ulong)input;
        }

        public string Convert()
        {
            m_number = ConvertToUlong(Input);

            if (m_number < 1000) return m_number.ToString();
            else if (m_number < 1000000) // million
            {
                var ret = (decimal)m_number / 1000m;
                return Math.Round(ret, 2).ToString().EndsWith("0") ? Math.Round(ret, 2).ToString().Trim('0') + ending[1] : Math.Round(ret, 2).ToString() + ending[1];
            }
            else if (m_number < 1000000000) // billion
            {
                var ret = (decimal)m_number / 1000000m;
                return Math.Round(ret, 2).ToString().EndsWith("0") ? Math.Round(ret, 2).ToString().Trim('0') + ending[2] : Math.Round(ret, 2).ToString() + ending[2];
            }
            else if (m_number < 1000000000000) // trillion
            {
                var ret = (decimal)m_number / 1000000000m;
                return Math.Round(ret, 2).ToString().EndsWith("0") ? Math.Round(ret, 2).ToString().Trim('0') + ending[3] : Math.Round(ret, 2).ToString() + ending[3];
            }
            else if (m_number < 1000000000000000) // I don't even know anymore
            {
                var ret = (decimal)m_number / 1000000000000m;
                return Math.Round(ret, 2).ToString().EndsWith("0") ? Math.Round(ret, 2).ToString().Trim('0') + ending[4] : Math.Round(ret, 2).ToString() + ending[4];
            }
            else return m_number.ToString();
        }
    }
}
