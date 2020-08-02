﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SEEL.LinguisticProcessor.Splitters
{
    public class CommaSplitter : ISplitter
    {
        public string Split(string input)
        {
            return String.Join( " ", input.Split(',') );
        }
    }
}