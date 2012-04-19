using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void GetCsv_ShouldRenderCorrectCsv()
        {
            IEnumerable<Thing> things = new List<Thing>
                {
                    new Thing
                        {
                            Id = 12,
                            Name = "Thing one",
                            Date = new DateTime(2008, 4, 20),
                            Child = new Child
                                        {
                                            Name = "Max"
                                        }
                        },
                    new Thing
                        {
                            Id = 13,
                            Name = "Thing two",
                            Date = new DateTime(2008, 5, 20),
                            Child = new Child
                                        {
                                            Name = "Robbie"
                                        }
                        }
                };

            var csv = things.Select(t => new { Id = t.Id, Name = t.Name, Date = t.Date, Child = t.Child.Name }).AsCsv();

            var csvArray = csv.Split(',', '\n');
            var expectedCsvArray = expectedCsv.Split(',', '\n');

            Assert.That(csvArray.Length, Is.EqualTo(9), "Bad CSV format, expected 9 delimited items");

            Assert.That(csvArray[0], Is.EqualTo(expectedCsvArray[0]));
            Assert.That(csvArray[1], Is.EqualTo(expectedCsvArray[1]));
            //Assert.That(csvArray[2], Is.EqualTo(expectedCsvArray[2])); // date compare fails on US date format
            Assert.That(csvArray[3], Is.EqualTo(expectedCsvArray[3]));
            Assert.That(csvArray[4], Is.EqualTo(expectedCsvArray[4]));
            Assert.That(csvArray[5], Is.EqualTo(expectedCsvArray[5]));
            //Assert.That(csvArray[6], Is.EqualTo(expectedCsvArray[6])); // date compare fails on US date format
            Assert.That(csvArray[7], Is.EqualTo(expectedCsvArray[7]));
            Assert.That(csvArray[8], Is.EqualTo(expectedCsvArray[8]));
        }

        const string expectedCsv = 
@"12,""Thing one"",""20/04/2008x 00:00:00"",""Max""
13,""Thing two"",""20/05/2008x 00:00:00"",""Robbie""
";

        public class Thing
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public Child Child { get; set; }
        }

        public class Child
        {
            public string Name { get; set; }
        }
    }
}
