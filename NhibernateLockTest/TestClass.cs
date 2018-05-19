using FluentNHibernate.Mapping;
using System;

namespace NhibernateLockTest
{
    public class TestClass
    {
        public virtual string Id { get; set; }

        public virtual string Title { get; set;  }

        public virtual DateTime Date { get; set; }

        public virtual int Number { get; set; }

        public TestClass()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class TestMap : ClassMap<TestClass>
    {
        public TestMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Title);
            Map(x => x.Date);
            Map(x => x.Number);
        }
    }
}
