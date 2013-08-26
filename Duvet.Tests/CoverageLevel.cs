using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Duvet;

namespace Duvet.Tests
{
    [TestFixture]
    public class CoverageLevelTests
    {
        public class OrTestValue
        {
            public CoverageLevel Expected;
            public CoverageLevel WhenEqual;
            public CoverageLevel OrdWith;

            public override string ToString()
            {
                return string.Format("{0} Or'd with {1} should be {2}", WhenEqual, OrdWith, Expected);
            }
        }

        private IEnumerable<OrTestValue> EmptyOrTestValues
        {
            get
            {
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.Empty,
                        WhenEqual = CoverageLevel.Empty,
                        OrdWith = CoverageLevel.Empty
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.FullyCovered,
                        WhenEqual = CoverageLevel.Empty,
                        OrdWith = CoverageLevel.FullyCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.NotCovered,
                        WhenEqual = CoverageLevel.Empty,
                        OrdWith = CoverageLevel.NotCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.Empty,
                        OrdWith = CoverageLevel.PartiallyCovered
                    };
            }
        }

        private IEnumerable<OrTestValue> FullyCoveredOrTestValues
        {
            get
            {
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.FullyCovered,
                        WhenEqual = CoverageLevel.FullyCovered,
                        OrdWith = CoverageLevel.Empty
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.FullyCovered,
                        WhenEqual = CoverageLevel.FullyCovered,
                        OrdWith = CoverageLevel.FullyCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.FullyCovered,
                        OrdWith = CoverageLevel.NotCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.FullyCovered,
                        OrdWith = CoverageLevel.PartiallyCovered
                    };
            }
        }

        private IEnumerable<OrTestValue> PartiallyCoveredOrTestValues
        {
            get
            {
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.PartiallyCovered,
                        OrdWith = CoverageLevel.Empty
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.PartiallyCovered,
                        OrdWith = CoverageLevel.FullyCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.PartiallyCovered,
                        OrdWith = CoverageLevel.NotCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.PartiallyCovered,
                        OrdWith = CoverageLevel.PartiallyCovered
                    };
            }
        }

        private IEnumerable<OrTestValue> NotCoveredOrTestValues
        {
            get
            {
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.NotCovered,
                        WhenEqual = CoverageLevel.NotCovered,
                        OrdWith = CoverageLevel.Empty
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.NotCovered,
                        OrdWith = CoverageLevel.FullyCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.NotCovered,
                        WhenEqual = CoverageLevel.NotCovered,
                        OrdWith = CoverageLevel.NotCovered
                    };
                yield return
                    new OrTestValue()
                    {
                        Expected = CoverageLevel.PartiallyCovered,
                        WhenEqual = CoverageLevel.NotCovered,
                        OrdWith = CoverageLevel.PartiallyCovered
                    };
            }
        }

        private IEnumerable<OrTestValue> OrTestValues
        {
            get {
                return new[] {PartiallyCoveredOrTestValues, FullyCoveredOrTestValues, NotCoveredOrTestValues, EmptyOrTestValues}.SelectMany(valueCollection => valueCollection);
            }
        }

        [Test]
        public void Or([ValueSource("OrTestValues")] OrTestValue testValue)
        {
            Assert.AreEqual(testValue.Expected, testValue.WhenEqual | testValue.OrdWith);
        }
    }
}
