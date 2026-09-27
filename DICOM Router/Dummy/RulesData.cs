using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router.Dummy
{
    class RulesData
    {
        public string ruleName { get; set; }
        public string ruleType { get; set; }
        public string firstDicomTag { get; set; }
        public string secondDicomTag { get; set; }
        public string replaceValue { get; set; }
        public string matchValue { get; set; }

        public RulesData(string ruleName, string ruleType, string firstDicomTag, string secondDicomTag, string replaceValue, string matchValue)
        {
            this.ruleName = ruleName;
            this.ruleType = ruleType;
            this.firstDicomTag = firstDicomTag;
            this.secondDicomTag = secondDicomTag;
            this.replaceValue = replaceValue;
            this.matchValue = matchValue;

        }

    }
}
